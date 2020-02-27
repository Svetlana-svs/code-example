package ru.softwerke.querybuilder.core.controller;

import org.apache.commons.lang3.text.WordUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.ModelAndView;
import ru.softwerke.querybuilder.core.constant.ApplicationConstants;
import ru.softwerke.querybuilder.core.data.dto.ControllerSettings;
import ru.softwerke.querybuilder.integration.jpa.config.ConnectionConfiguration;
import ru.softwerke.querybuilder.integration.jpa.dto.Connection;
import ru.softwerke.querybuilder.integration.jpa.repository.AdminRepository;

import javax.servlet.http.HttpServletRequest;
import java.util.Optional;

/*
 *  @author Svetlana Suvorova
 */
@RestController
public class PageController {

    @Autowired
    private final AdminRepository adminRepository;

    public PageController(AdminRepository adminRepository) {
        this.adminRepository = adminRepository;
    }

    @RequestMapping(value="/{path}", method = RequestMethod.GET)
    public ModelAndView getPage(HttpServletRequest request,
                                @PathVariable("path") String path,
                                @RequestParam Optional<String> error) {
        ControllerSettings controllerSettings = new ControllerSettings(request.getContextPath());

        return new ModelAndView(path, ApplicationConstants.ERROR_PAGE, error)
                .addObject("settings", controllerSettings.toJson());
    }

    @RequestMapping(value={"/queryFormBuilder/{action}/{id}", "/queryFormBuilder/{action}"}, method = RequestMethod.GET)
    public ModelAndView getQueryFormBuilderPage(HttpServletRequest request,
                                                @PathVariable("action") String action,
                                                @PathVariable("id") Optional<String> id,
                                                @RequestParam Optional<String> metaDb,
                                                @RequestParam Optional<String> error) {

        ControllerSettings controllerSettings = new ControllerSettings(request.getContextPath());

        // TODO: create parent class extends from ModelAndView with controllerSettings values
        final String connectionMdbAlias = Optional.ofNullable(
                ConnectionConfiguration.getConnections().stream()
                        .filter(Connection::isConnectionMdb)
                        .filter(c -> c.getAlias().equals(
                                Optional.ofNullable(metaDb).filter(Optional::isPresent)
                                        .map(d -> metaDb.get()).orElse("")))
                        .findFirst())
                .filter(Optional::isPresent)
                .map(c -> c.get().getAlias()).orElse("");

        return new ModelAndView("queryFormBuilder", ApplicationConstants.ERROR_PAGE, error)
                .addObject("settings", controllerSettings.toJson())
                .addObject("id", Optional.ofNullable(id).filter(Optional::isPresent).map(i -> i.get().toString()).orElse(""))
                .addObject("connectionMdb", connectionMdbAlias);
    }

    @RequestMapping(value={"/userForm/{action}/{id}"}, method = RequestMethod.GET)
    public ModelAndView getUserFormPage(HttpServletRequest request,
                                                @PathVariable("action") String action,
                                                @PathVariable("id") String id,
                                                @RequestParam Optional<String> error) {

        ControllerSettings controllerSettings = new ControllerSettings(request.getContextPath());

        return new ModelAndView("query" + WordUtils.capitalize(action), ApplicationConstants.ERROR_PAGE, error)
                .addObject("settings", controllerSettings.toJson())
                .addObject("id", id);
    }
}
