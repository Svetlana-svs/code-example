S.MainNamespace.UploadQRReceipt = (function() {

    S.MainNamespace.AbstractComponent.call();

    var DEFAULT_QR_CODE_FORMAT_VALIDATION_RULE = '^t=.*fn=.*fp=.*$';
    var VALIDATION_IGNORE_CLASS_NAME = 'validate-ignore';
    var BYTES_IN_KB = 1024;
    var PERMISSIBLE_RELATIVE_FREQUENCY = 0.8;

    var ScanningType = {
        VIDEO: 'video',
        FILE: 'file'
    };

    var uploadQRReceiptForm = function(settings) {

        var $container = null;
        var $successMessage = null;
        var $errorMessage= null ;
        var $infoMessage = null;

        var $form = null;
        var $componentModal = null;

        var scanType = ScanningType.FILE;

        // Scanning
        var $btnScanningStart = null;
        var $btnScanRemove = null;
        var $videoScan = null;
        var $scanningResult = null;

        var qrScanner = null;
        var qrCode = '';
        var scanSnap = null;
        var scanResults = [];
        var currentDecodeIteration = 0;

        // File uploading
        var $receiptFileInput = null;

        //constructor
        $(function() {
            if (!settings) return;
            initElements();
            setListeners();

            if (settings.qrScannerEnabled) {
                 initScanning();
            }

            setValidation();
        });

        function initElements() {
            $container = $('#uploadQRReceipt_' + settings.instanceId);
            if (!$container.length) {
                console.error('One of the necessary DOM elements not found');
                return;
            }

            $successMessage = $container.find('.message--success');
            $errorMessage = $container.find('.message--error');
            $infoMessage = $container.find('.message--info');
            $componentModal = $container.find('.uploadQRReceiptModal');
            $form = (settings.formRenderMode === 'Modal') ? $componentModal.find('form') : $container.find('form');

            $receiptFileInput =  $form.find('.receiptFileInput');
            $receiptPreviewModal = $container.find('.uploadQRReceiptPreviewZoom');
        }

        function setListeners() {
            var $btnUploadQRReceiptStart = $container.find('.uploadQRReceiptStart');
            if ($btnUploadQRReceiptStart && $btnUploadQRReceiptStart.length) {
                $btnUploadQRReceiptStart.on('click', function() {
                    if (!S.MainNamespace.CookieController.isUserLoggedIn()){
                        S.MainNamespace.LoginModalController.openLoginModal();
                        return false;
                    }
                    if ((settings.formRenderMode === 'Modal')) {
                        $componentModal.modal('show');
                    }
                });
            }

            $receiptFileInput.on('change', function() {
                scanFile(this);
            });

            $receiptFileInput.on('click', function(e) {
                clearMessage();
                clearUploadFileForm();
                checkUserAuthorization(null, function(e) {
                    e.preventDefault();
                });
            });

            $form.find('.file-preview').on('click', function() {
                $receiptPreviewModal.modal('show');
            });

            $form.find('.btnFileRemove').on('click', function() {
                clearUploadFileForm();
            });
        }

        function initScanning() {
            navigator.getWebcam = (navigator.getUserMedia ||
                navigator.webKitGetUserMedia || navigator.moxGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia);

            currentDecodeIteration = settings.decodeIterations;

            $videoScan = $form.find('.scanningVideo');
            $btnScanRemove = $form.find('.btnScanRemove');
            $btnScanningStart = $form.find('.btnScanningStart');
            $scanningResult = $form.find('.scanningResult');

            scanType = ScanningType.VIDEO;

            qrScanner = new QrScanner('video', $videoScan[0], function(result) {
                processQRScanningResult(result);
            });

            $btnScanningStart.on('click', function() {
                checkUserAuthorization(initScanVideoStream, null);
            });

            $btnScanRemove.on('click', function() {
                clearScanningForm();
                clearMessage();
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function(e) {
                clearMessage();
                scanType = (e.target.href.indexOf('tabVideoScanner') !== -1) ? ScanningType.VIDEO : ScanningType.FILE;
                if ((scanType == ScanningType.FILE) && $videoScan && ($videoScan.length  > 0)) {
                    var playPromise =  $videoScan[0].play();

                    if ((playPromise !== undefined) && $videoScan[0].played.length) {
                        $videoScan[0].pause();
                    }
                }
                if ((scanType == ScanningType.VIDEO) && $videoScan && ($videoScan.length  > 0)) {
                    $videoScan[0].play();
                }
                changeValidation();
            });
        }

        function initScanVideoStream() {
            if (navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia( {video: { facingMode: 'environment' } } )
                    .then(function(stream) {

                        if (typeof ($videoScan[0].srcObject) !== 'undefined') {
                            $videoScan[0].srcObject = stream;
                        }
                        else {
                            $videoScan.attr('src', URL.createObjectURL(stream));
                        }
                        $videoScan[0].play();
                        $btnScanningStart.hide();

                    })
                    .catch(function(error) {
                        $errorMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.scan.camera.error')).show();
                    });
            } else {
                navigator.getWebcam({video: { facingMode: 'environment' } },
                    function(stream) {
                        if (typeof ($videoScan[0].srcObject) !== 'undefined') {
                            $videoScan[0].srcObject = stream;
                        }
                        else {
                            $videoScan.attr('src', URL.createObjectURL(stream));
                        }
                        $videoScan[0].play();
                        $btnScanningStart.hide();
                    },
                    function () { logError('Web cam is not accessible.'); });
            }
        }

        function processQRScanningResult(result) {
            scanResults.push(result);
            if (checkScanResults()) {
                if (checkTypeOfResult(qrCode)) {
                    setQrScanningResult();
                    clearUploadFileForm();
                    currentDecodeIteration = 0;
                } else {
                    $videoScan[0].pause();
                    scanResults = [];
                    $errorMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.scan.type.error')).show();
                }
                $btnScanRemove.show();
            }
            if (scanResults.length == 1) {
                // Clear messages by scanning begin by timeout to prevent blinking message
                setTimeout(clearMessage, 100);
            }
        }

        function checkScanResults() {
            if (scanResults.length > 4) {
                var scanResultsMode = scanResults.reduce(function(obj, result) {
                    obj[result] ? obj[result]++ : (obj[result] = 1);
                    return obj;
                }, {});

                if (Object.keys(scanResultsMode).length > 1) {
                    // If scanning results is different
                    var scanResultsSort = [];
                    Object.keys(scanResultsMode).forEach(function(key) {
                        scanResultsSort.push({result: key, count: scanResultsMode[key]});
                    });
                    scanResultsSort.sort(function(a, b) {
                        return b.count - a.count;
                    });

                    var relativeFrequency = (scanResultsSort[0].count / scanResults.length).toFixed(2);

                    if (relativeFrequency < PERMISSIBLE_RELATIVE_FREQUENCY) {
                        return false;
                    }
                    qrCode = scanResultsSort[0].result;
                } else {
                     qrCode = scanResults[0];
                }

                return true;
            }

            return false;
        }

        function scanFile(input) {
            readImageURL(input);
            if (settings.qrScannerEnabled) {
                clearMessage();
                clearScanningForm();

                var fileScanner = new QrScanner('file', input.files[0], function (result) {
                    if (checkTypeOfResult(result)) {
                        qrCode = result;
                        console.log("File scanning: " + result);
                        currentDecodeIteration = 0;
                    } else {
                        $errorMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.scan.type.error')).show();
                    }

                }, function() {
                    console.log("No qr code found.");
                    if (currentDecodeIteration > 0) {
                        currentDecodeIteration--;
                    }
                    if (currentDecodeIteration === 0) {
                        $infoMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.file.scan.hand.processing')).show();
                    } else {
                        $errorMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.file.scan.error')).show();
                    }

                });
            }
        }

        function readImageURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function(e) {
                    $form.find('.receipt-container').show('slow');
                    $form.find('.preview-image').attr('src', e.target.result);
                    $form.find('.file-name').text(input.files[0].name);
                    $receiptPreviewModal.find('.preview-image').attr('src', e.target.result);
                };

                reader.readAsDataURL(input.files[0]);
            }
        }

        function checkIsPhoneConfirmed() {
            S.MainNamespace.EditPhoneControllerPublic.getUserPhoneNumber(
                function success(phoneNumber, phoneNumberConfirmed) {
                    return phoneNumber && phoneNumberConfirmed;
                },
                function fail(data) {
                    return false;
                });
        }

        function checkUserAuthorization(successCallback, failureCallback) {
            // user logged in?
            if (!S.MainNamespace.CookieController.isUserLoggedIn()) {
                $errorMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.authorization.required')).show();
                if (failureCallback) {
                    failureCallback(event);
                    return;
                }
            } else {
                if (settings.phoneConfirmationRequired && (settings.phoneConfirmationRequired === 'beforeUpload')) {
                    if (!checkIsPhoneConfirmed()) {
                        S.MainNamespace.EditPhoneModalController.openEditPhoneModal(null,
                                S.MainNamespace.I18n.get('uploadqrreceipt.phoneconfirmation.modal.heading'),
                                S.MainNamespace.I18n.get('uploadqrreceipt.phoneconfirmation.modal.text'));
                                // user will have to click order button again
                        if (failureCallback) {
                            failureCallback(event);
                        }
                        return;
                    }
                }

                if (successCallback) {
                    successCallback(event);
                    return;
                }
            }
        }

        function checkTypeOfResult(result) {
            var qrCodeFormatValidRule = new RegExp(settings.qrCodeFormatValidRule || DEFAULT_QR_CODE_FORMAT_VALIDATION_RULE);
            return result.match(qrCodeFormatValidRule);
        }

        function setQrScanningResult() {
            $videoScan[0].pause();
            scanSnap = qrScanner.makePicture();
            $scanningResult.val(qrCode);
            $scanningResult.addClass('result-decor');
        }

        function setValidation() {
            $form.validate({
                ignore: function (index, field) {
                    if ($(field).hasClass(VALIDATION_IGNORE_CLASS_NAME)) {
                        return true;
                    }
                },
                submitHandler: function (form) {
                    submitForm(form);
                },
                messages: {
                    file: {
                        required: S.MainNamespace.I18n.get('uploadqrreceipt.form.validation.file.required'),
                        filesize: S.MainNamespace.I18n.get('uploadqrreceipt.form.validation.file.filesize',
                            [Math.round(settings.fileMaxSize / (BYTES_IN_KB * BYTES_IN_KB))]),
                    },
                    scanningResult: {
                        required: S.MainNamespace.I18n.get('uploadqrreceipt.form.validation.scan.required'),
                    },
                    text: {
                        required: S.MainNamespace.I18n.get('uploadqrreceipt.form.validation.text'),
                    }
                }
            });

            $.validator.addMethod('acceptedTypeCheck', function(value, element) {
                var extension = value.substring(value.lastIndexOf('.') + 1);
                var fileAcceptedExtensions = null;

                if (settings.fileAcceptedExtensions) {
                    fileAcceptedExtensions = settings.fileAcceptedExtensions.toLowerCase().replace(/\s/g, '').split(',');
                }
                if ((fileAcceptedExtensions && $.inArray(extension.toLowerCase(), fileAcceptedExtensions) !== -1) ||
                        !fileAcceptedExtensions) {
                    return true;
                }

                return false;
            }, S.MainNamespace.I18n.get('uploadqrreceipt.form.file.format.error'));

            if (S.MainNamespace.CookieController.isUserLoggedIn()) {
                // Validation of a not authorized user is with checkUserAuthorization method to display special message
                $receiptFileInput.rules('add', {
                    required: true,
                    acceptedTypeCheck: true
                });
                if (settings.qrScannerEnabled) {
                    $scanningResult.rules('add', {
                        required: true
                    });
                    changeValidation();
                }
           }
        }

        function changeValidation() {
            if (scanType == ScanningType.FILE) {
                $scanningResult.addClass(VALIDATION_IGNORE_CLASS_NAME);
                $receiptFileInput.removeClass(VALIDATION_IGNORE_CLASS_NAME);
            } else {
                $receiptFileInput.addClass(VALIDATION_IGNORE_CLASS_NAME);
                $scanningResult.removeClass(VALIDATION_IGNORE_CLASS_NAME);
            }
        }

        function clearMessage() {
            $successMessage.hide().text('');
            $errorMessage.hide().text('');
            $infoMessage.hide().text('');
        }

        function clearUploadFileForm() {
            $receiptFileInput.val('');
            $form.find('.file-name').text('');
            $form.find('.preview-image').attr('src', '');
            $form.find('.receipt-container').hide();
            $('label[for^="receiptFileInput"].error').hide();
        }

        function clearScanningForm() {
            if (settings.qrScannerEnabled) {
                $videoScan[0].play();
                scanSnap = null;
                $scanningResult.val('');
                $scanningResult.removeClass('result-decor');
                $btnScanRemove.hide();
                scanResults = [];
            }
        }

        function submitForm(form) {
            if (!S.MainNamespace.CookieController.isUserLoggedIn()) {
                S.MainNamespace.LoginModalController.openLoginModal();
                return;
            }

            if (settings.phoneConfirmationRequired && (settings.phoneConfirmationRequired === 'beforeSubmit')) {
                if (!checkIsPhoneConfirmed()) {
                    S.MainNamespace.EditPhoneModalController.openEditPhoneModal(null,
                            S.MainNamespace.I18n.get('uploadqrreceipt.phoneconfirmation.modal.heading'),
                            S.MainNamespace.I18n.get('uploadqrreceipt.phoneconfirmation.modal.text'));
                            // user will have to click order button again
                    return;
                }
            }
            clearMessage();

            if (currentDecodeIteration !== 0) {
                $errorMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.file.scan.error')).show();
                return;
            }

            var formData = new FormData(form);
            formData.append('promoId', S.MainNamespace.Utils.getPromoId());
            formData.append('qrCode', qrCode);
            formData.append('scannedReceipt', (scanType == ScanningType.VIDEO) ? scanSnap : '');

            $.ajax({
                url: '/...',
                type: 'POST',
                cache: false,
                data: formData,
                processData: false,
                contentType: false
            })
            .done(function (data, statusText, xhr) {
                if (data.success) {

                    $successMessage.text(S.MainNamespace.I18n.get('uploadqrreceipt.form.success')).show();
                    S.MainNamespace.Mediator.publish(S.MainNamespace.Mediator.channels.RECEIPT_UPLOADED);

                    // Adobe Analytics
                    if (S.AnalyticsNamespace.DirectCallFunctions &&
                        S.AnalyticsNamespace.DirectCallFunctions.uploadReceipt) {
                        var eventData = data.analyticsEventData ? data.analyticsEventData : {};
                        S.AnalyticsNamespace.DirectCallFunctions.uploadReceipt(eventData.receiptId);
                    }

                } else {

                    $errorMessage.text(S.MainNamespace.I18n.get(
                            'error.crm.' + ((data && data.errorCode) ? (data.errorCode) : 'unknown'))).show();

                    // Adobe Analytics
                    if (S.AnalyticsNamespace.DirectCallFunctions &&
                        S.AnalyticsNamespace.DirectCallFunctions.uploadReceiptFailure) {
                        S.AnalyticsNamespace.DirectCallFunctions.uploadReceiptFailure(
                            'Receipt upload failure - server error', data.errorCode);
                    }
                }

                // clear form
                qrCode = '';
                currentDecodeIteration = settings.decodeIterations;
                var textField = $container.find('.uploadReceiptText');
                if (textField && (textField.length > 0)) {
                    textField.val('');
                }
                if (scanType == ScanningType.FILE) {
                    clearUploadFileForm();
                } else {
                    clearScanningForm();
                }
            })
            .fail(function() {
                $errorMessage.text(S.MainNamespace.I18n.get('error.server.internal')).show();

                // Adobe Analytics
                if (S.AnalyticsNamespace.DirectCallFunctions &&
                    S.AnalyticsNamespace.DirectCallFunctions.uploadReceiptFailure) {
                    S.AnalyticsNamespace.DirectCallFunctions.uploadReceiptFailure(
                        'Receipt upload failure - request error', jqXHR.status + ': ' + jqXHR.statusText);
                }
            });
        }
    };

    return {
        uploadQRReceiptForm: uploadQRReceiptForm
    };
})($);