package ru.softwerke.querybuilder.core.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.stereotype.Component;
import ru.softwerke.querybuilder.core.service.UserRoleService;
import ru.softwerke.querybuilder.integration.jpa.entity.Benutzer;
import ru.softwerke.querybuilder.integration.jpa.repository.service.ConnectionService;

import java.util.ArrayList;
import java.util.List;

/*
 *  @author Svetlana Suvorova
 */
@Component
public class ApplicationAuthenticationProvider implements AuthenticationProvider {

    @Autowired
    private UserRoleService userRoleService;

    @Override
    public Authentication authenticate(Authentication authentication) throws AuthenticationException {
        String userName = authentication.getName();
        boolean isRoleAdmin = UserRoleConfiguration.getUsers().get(Role.ROLE_ADMIN)
                .stream().anyMatch(u -> u.getUserName().equals(userName));
        if (isRoleAdmin) {
            return null;
        }
        String password = authentication.getCredentials().toString();

        Benutzer user = userRoleService.getUserByName(userName);
        String decodePassword = ConnectionService.decryptString(user.getPassword());

        List<GrantedAuthority> authorities = new ArrayList<>();
        authorities.add(new SimpleGrantedAuthority(Role.ROLE_USER.getName()));

        if (userName.equals(user.getName()) && password.equals(decodePassword)) {
            return new UsernamePasswordAuthenticationToken(userName, password, authorities);
        } else {
            throw new BadCredentialsException("Authentication failed");
        }
    }

    @Override
    public boolean supports(Class<?> aClass) {
        return aClass.equals(UsernamePasswordAuthenticationToken.class);
    }
}