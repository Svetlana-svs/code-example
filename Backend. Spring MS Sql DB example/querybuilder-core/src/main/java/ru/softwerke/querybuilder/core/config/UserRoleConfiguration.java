package ru.softwerke.querybuilder.core.config;

import org.apache.commons.lang3.StringUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Configuration;
import org.springframework.validation.annotation.Validated;
import ru.softwerke.querybuilder.core.service.UserRoleService;
import ru.softwerke.querybuilder.integration.jpa.dto.Credential;
import ru.softwerke.querybuilder.integration.jpa.entity.Benutzer;
import ru.softwerke.querybuilder.integration.jpa.repository.service.ConnectionService;

import javax.annotation.PostConstruct;
import javax.validation.constraints.NotNull;
import java.util.*;
import java.util.stream.Collectors;

/*
 *  @author Svetlana Suvorova
 */
@Configuration
@Validated
@ConfigurationProperties(prefix = "app.auth")
public class UserRoleConfiguration {

    @Autowired
    private UserRoleService userRoleService;

    @NotNull
    private String roleAdmin;

    private static Map<Role, List<Credential>> users = new HashMap<>();

    public UserRoleConfiguration(UserRoleService userRoleService) {
        this.userRoleService = userRoleService;
    }

    @PostConstruct
    public void setUp() {

        // Get all admin users as String and collect to users list
        Set<String> adminNames = Arrays.stream(roleAdmin.split(","))
                .distinct()
                .filter(StringUtils::isNotBlank)
                .collect(Collectors.toSet());

        List<Benutzer> adminList = userRoleService.getUserAdminRoleList(adminNames);

        users.put(Role.ROLE_ADMIN, adminList.stream()
                .filter(b -> StringUtils.isNotBlank(b.getPassword()))
                .map(b -> new Credential(b.getName(), ConnectionService.decryptString(b.getPassword())))
                .collect(Collectors.toList()));
    }

    static Map<Role, List<Credential>> getUsers() {
        return users;
    }

    public String getRoleAdmin() {
        return roleAdmin;
    }

    public void setRoleAdmin(String roleAdmin) {
        this.roleAdmin = roleAdmin;
    }

    public static void setUsers(Map<Role, List<Credential>> users) {
        UserRoleConfiguration.users = users;
    }
}
