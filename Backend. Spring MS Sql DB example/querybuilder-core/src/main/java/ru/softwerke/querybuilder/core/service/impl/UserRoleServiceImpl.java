package ru.softwerke.querybuilder.core.service.impl;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.softwerke.querybuilder.core.service.MetadataService;
import ru.softwerke.querybuilder.core.service.UserRoleService;
import ru.softwerke.querybuilder.integration.jpa.config.ConnectionConfiguration;
import ru.softwerke.querybuilder.integration.jpa.context.ConnectionContextHolder;
import ru.softwerke.querybuilder.integration.jpa.dto.Connection;
import ru.softwerke.querybuilder.integration.jpa.dto.User;
import ru.softwerke.querybuilder.integration.jpa.entity.Benutzer;
import ru.softwerke.querybuilder.integration.jpa.repository.UserRepository;

import java.util.List;
import java.util.Optional;
import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

/*
 *  @author Svetlana Suvorova
 */
@Service
public class UserRoleServiceImpl implements UserRoleService {

    @Autowired
    private UserRepository userRepository;
    @Autowired
    private MetadataService metadataService;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    public UserRoleServiceImpl(UserRepository userRepository, MetadataService metadataService) {
        this.userRepository = userRepository;
        this.metadataService = metadataService;
    }

    @Override
    public List<Benutzer> getUserAdminRoleList(Set<String> adminNames) {

        String connectionMdb = getConnectionMdbDefault();

        return getUserAdminRoleList(connectionMdb, adminNames);
    }

    @Override
    public List<Benutzer> getUserAdminRoleList(String connectionMdb, Set<String> adminNames) {

        String connectionMdbAlias = metadataService.getConnectionMdb(connectionMdb);

        ConnectionContextHolder.set(connectionMdbAlias);
        List<Benutzer> adminList = userRepository.findAllUserByName(adminNames);

        return adminList;
    }

    @Override
    public List<User> getUserList(Set<Integer> userIds) {

        String connectionMdb = getConnectionMdbDefault();

        return getUserList(connectionMdb, userIds);
    }

    @Override
    public List<User> getUserList(String connectionMdb, Set<Integer> userIds) {

        String connectionMdbAlias = metadataService.getConnectionMdb(connectionMdb);

        ConnectionContextHolder.set(connectionMdbAlias);
        List<User> userList = userRepository.findAllUserById(userIds);
        ConnectionContextHolder.clear();

        return userList;
    }

    public List<Benutzer> getUserUserRoleList() {

        String connectionMdb = getConnectionMdbDefault();

        return getUserUserRoleList(connectionMdb);
    }

    @Override
    public List<Benutzer> getUserUserRoleList(String connectionMdb) {

        String connectionMdbAlias = metadataService.getConnectionMdb(connectionMdb);

        ConnectionContextHolder.set(connectionMdbAlias);
        List<Benutzer> userList = userRepository.findAllActiveUser();
        ConnectionContextHolder.clear();

        return userList;
    }

    @Override
    public List<User> getAllUserList(String connectionMdb) {

        String connectionMdbAlias = metadataService.getConnectionMdb(connectionMdb);

        ConnectionContextHolder.set(connectionMdbAlias);
        List<User> userList = userRepository.findAllUser();
        ConnectionContextHolder.clear();

        return userList;
    }

    @Override
    public List<User> getAllUserList() {

        String connectionMdb = getConnectionMdbDefault();

        return getAllUserList(connectionMdb);
    }

    @Override
    public Benutzer getUserByName(String userName) {

        String connectionMdb = getConnectionMdbDefault();

        ConnectionContextHolder.set(connectionMdb);
        List<Benutzer> userList = userRepository.findAllUserByName(Stream.of(userName).collect(Collectors.toSet()));
        ConnectionContextHolder.clear();
        if (userList.size() != 1) {
            return null;
        }

        return userList.get(0);
    }

    private String getConnectionMdbDefault() {

        return Optional.ofNullable(
                ConnectionConfiguration.getConnections().stream()
                        .filter(Connection::isDefaultConnectionMdb)
                        .findFirst())
                .filter(Optional::isPresent)
                .map(c -> c.get().getAlias()).orElse(null);
    }
}
