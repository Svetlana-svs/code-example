package ru.softwerke.querybuilder.core.service;

import ru.softwerke.querybuilder.integration.jpa.dto.User;
import ru.softwerke.querybuilder.integration.jpa.entity.Benutzer;

import java.util.List;
import java.util.Set;

/*
 *  @author Svetlana Suvorova
 */
public interface UserRoleService {

    List<Benutzer> getUserAdminRoleList(Set<String> adminNames);

    List<Benutzer> getUserAdminRoleList(String connectionMdb, Set<String> adminNames);

    List<Benutzer> getUserUserRoleList();

    List<Benutzer> getUserUserRoleList(String connectionMdb);

    List<User> getUserList(Set<Integer> userIds);

    List<User> getUserList(String connectionMdb, Set<Integer> userIds);

    List<User> getAllUserList();

    List<User> getAllUserList(String connectionMdb);

    Benutzer getUserByName(String userName);
}
