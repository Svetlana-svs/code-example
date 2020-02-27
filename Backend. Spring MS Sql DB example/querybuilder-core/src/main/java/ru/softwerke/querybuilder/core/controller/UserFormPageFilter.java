package ru.softwerke.querybuilder.core.controller;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.savedrequest.HttpSessionRequestCache;
import org.springframework.security.web.savedrequest.RequestCache;
import org.springframework.stereotype.Component;
import org.springframework.web.context.support.SpringBeanAutowiringSupport;
import org.springframework.web.filter.GenericFilterBean;
import ru.softwerke.querybuilder.core.config.Role;
import ru.softwerke.querybuilder.core.data.queryForm.QueryForm;
import ru.softwerke.querybuilder.core.exception.QueryFormIOException;
import ru.softwerke.querybuilder.core.service.UserRoleService;
import ru.softwerke.querybuilder.core.service.impl.QueryFormServiceImpl;
import ru.softwerke.querybuilder.integration.jpa.dto.ConnectionType;
import ru.softwerke.querybuilder.integration.jpa.dto.User;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashSet;
import java.util.List;
import java.util.stream.Collectors;

@Component
public class UserFormPageFilter extends GenericFilterBean {

    @Autowired
    private final QueryFormServiceImpl queryFormService;

    @Autowired
    private final UserRoleService userService;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    public UserFormPageFilter(QueryFormServiceImpl queryFormService, UserRoleService userService) {
        this.queryFormService = queryFormService;
        this.userService = userService;
    }

    @Override
    public void initFilterBean() throws ServletException{

        SpringBeanAutowiringSupport.processInjectionBasedOnServletContext(this, getServletContext());
    }

    @Override
    public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {

        if (((HttpServletRequest)request).getRequestURI().contains("userForm")) {

            String[] uriPaths = ((HttpServletRequest)request).getRequestURI()
                    .substring(((HttpServletRequest)request).getRequestURI().indexOf("userForm") + 9).split("/");
            List<String> idList = Arrays.stream(uriPaths)
                    .filter(s -> !(s.equals("filter") || s.equals("list") || s.equals("details")))
                    .collect(Collectors.toList());
            if (idList.size() != 1) {
                chain.doFilter(request, response);
            } else {
                String formId = idList.get(0);
                try {
                    QueryForm queryForm = queryFormService.getQueryForm(formId);

                    if (queryForm.getInfo().isAuthenticated()) {
                        if (SecurityContextHolder.getContext().getAuthentication() != null &&
                                SecurityContextHolder.getContext().getAuthentication().isAuthenticated()) {

                            List<User> queryFormUsers = userService.getUserList(queryForm.getInfo().getConnection().get(ConnectionType.MDB),
                                    new HashSet<>(queryForm.getInfo().getUsers()));

                            Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
                            Collection<? extends GrantedAuthority> authorities = authentication.getAuthorities();
                            boolean isRoleAdmin = authorities.stream()
                                    .anyMatch(r -> ((GrantedAuthority) r).getAuthority().equals( Role.ROLE_ADMIN.name()));
                            boolean isUserAccess = isRoleAdmin || queryFormUsers.stream()
                                    .anyMatch(u -> u.getName().equals(authentication.getName()));

                            if (isUserAccess) {
                                chain.doFilter(request, response);
                            } else {
                                ((HttpServletResponse)response).sendRedirect(((HttpServletRequest)request).getContextPath() + "/error");
                            }
                        } else {
                            RequestCache requestCache = new HttpSessionRequestCache();
                            requestCache.saveRequest((HttpServletRequest)request, (HttpServletResponse)response);
                            ((HttpServletResponse)response).sendRedirect(((HttpServletRequest)request).getContextPath() + "/login");
                        }
                    } else {
                        chain.doFilter(request, response);
                    }
                } catch (QueryFormIOException e) {
                    logger.error("UserFormPageFilter error by form get: %s", e.getMessage());
                    chain.doFilter(request, response);
                }
            }
        } else {
           chain.doFilter(request, response);
        }
    }
}
