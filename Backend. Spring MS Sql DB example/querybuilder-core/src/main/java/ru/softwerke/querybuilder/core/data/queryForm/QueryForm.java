package ru.softwerke.querybuilder.core.data.queryForm;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonView;
import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import com.fasterxml.jackson.databind.annotation.JsonSerialize;

import java.io.Serializable;
import java.util.List;

/*
 *  @author Svetlana Suvorova
 *
 *  Dto class for parsing json with query form data.
 *
 */
public class QueryForm implements Serializable {

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private Info info;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private String startPoint;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String cssFile;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String errorMessage;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private ResultOptions resultOptions;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private AdditionalInfoType additionalInfo;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private List<Field> filter;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private List<Field> list;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private List<Field> detail;

    public Info getInfo() {
        return info;
    }

    @JsonSerialize
    @JsonDeserialize
    public void setInfo(Info info) {
        this.info = info;
    }

    public String getStartPoint() {
        return startPoint;
    }

    public void setStartPoint(String startPoint) {
        this.startPoint = startPoint;
    }

    public String getCssFile() {
        return cssFile;
    }

    public void setCssFile(String cssFile) {
        this.cssFile = cssFile;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public void setErrorMessage(String errorMessage) {
        this.errorMessage = errorMessage;
    }

    public ResultOptions getResultOptions() {
        return resultOptions;
    }

    public void setResultOptions(ResultOptions resultOptions) {
        this.resultOptions = resultOptions;
    }

    public AdditionalInfoType getAdditionalInfo() {
        return additionalInfo;
    }

    public void setAdditionalInfo(AdditionalInfoType additionalInfo) {
        this.additionalInfo = additionalInfo;
    }

    public List<Field> getFilter() {
        return filter;
    }

    public void setFilter(List<Field> filter) {
        this.filter = filter;
    }

    public List<Field> getList() {
        return list;
    }

    public void setList(List<Field> list) {
        this.list = list;
    }

    public List<Field> getDetail() {
        return detail;
    }

    public void setDetails(List<Field> detail) {
        this.detail = detail;
    }

    public void setDetail(List<Field> detail) {
        this.detail = detail;
    }

    private static class ResultOptions implements Serializable {

        @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
        private boolean count;

        @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
        private boolean numbers;

        @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
        private boolean criteria;

        public boolean isCount() {
            return count;
        }

        public void setCount(boolean count) {
            this.count = count;
        }

        public boolean isNumbers() {
            return numbers;
        }

        public void setNumbers(boolean numbers) {
            this.numbers = numbers;
        }

        public boolean isCriteria() {
            return criteria;
        }

        public void setCriteria(boolean criteria) {
            this.criteria = criteria;
        }
    }

    private enum StartPointType {
        @JsonProperty("filter")
        FILTER,

        @JsonProperty("list")
        LIST,

        @JsonProperty("details")
        DETAILS;
    }

    private enum AdditionalInfoType {
        @JsonProperty("none")
        NONE,

        @JsonProperty("footnote")
        FOOTNOTE,

        @JsonProperty("reference")
        REFERENCE;
    }
}
