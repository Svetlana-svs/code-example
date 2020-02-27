package ru.softwerke.querybuilder.core.controller;

import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.filter.GenericFilterBean;
import ru.softwerke.querybuilder.core.config.Role;
import ru.softwerke.querybuilder.core.constant.ApplicationConstants;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.Collection;

public class LoginPageFilter extends GenericFilterBean {

    @Override
    public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {

        if (SecurityContextHolder.getContext().getAuthentication() != null &&
                SecurityContextHolder.getContext().getAuthentication().isAuthenticated() &&
                (((HttpServletRequest)request).getRequestURI().equals(((HttpServletRequest)request).getContextPath()) ||
                    ((HttpServletRequest)request).getRequestURI().equals(((HttpServletRequest)request).getContextPath() + "/") ||
                    ((HttpServletRequest)request).getRequestURI().endsWith("/login"))) {

            Collection<? extends GrantedAuthority> authorities = SecurityContextHolder.getContext().getAuthentication().getAuthorities();
            boolean isRoleAdmin = authorities.stream()
                    .anyMatch(r -> ((GrantedAuthority) r).getAuthority().equals( Role.ROLE_ADMIN.name()));
            if (isRoleAdmin) {
                ((HttpServletResponse)response).sendRedirect(
                        ((HttpServletRequest)request).getContextPath() + "/" + ApplicationConstants.HOME_PAGE);
            } else {
                chain.doFilter(request, response);
            }
        }
        else {
            chain.doFilter(request, response);
        }
    }
}