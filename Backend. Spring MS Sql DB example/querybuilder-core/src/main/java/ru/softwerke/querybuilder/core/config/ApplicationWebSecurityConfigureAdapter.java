package ru.softwerke.querybuilder.core.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.authentication.AnonymousAuthenticationFilter;
import org.springframework.security.web.authentication.www.BasicAuthenticationFilter;
import ru.softwerke.querybuilder.core.controller.LoginPageFilter;
import ru.softwerke.querybuilder.core.controller.LoginRedirectAuthenticationSuccessHandler;
import ru.softwerke.querybuilder.core.controller.UserFormPageFilter;
import ru.softwerke.querybuilder.core.service.UserRoleService;
import ru.softwerke.querybuilder.core.service.impl.QueryFormServiceImpl;

import java.util.function.Consumer;

/*
 *  @author Svetlana Suvorova
 */
@Configuration
@EnableWebSecurity
public class ApplicationWebSecurityConfigureAdapter extends WebSecurityConfigurerAdapter {

    @Autowired
    private ApplicationAuthenticationProvider applicationAuthenticationProvider;

    @Autowired
    private ApplicationConfiguration applicationConfiguration;

    @Autowired
    private UserRoleConfiguration userRoleConfiguration;

    @Autowired
    private final QueryFormServiceImpl queryFormService;

    @Autowired
    private final UserRoleService userService;

    static <T> Consumer<T> throwingConsumerWrapper(
            ThrowingConsumer<T, Exception> throwingConsumer) {
        return i -> {
            try {
                throwingConsumer.accept(i);
            } catch (Exception ex) {
                throw new RuntimeException(ex);
            }
        };
    }

    public ApplicationWebSecurityConfigureAdapter(QueryFormServiceImpl queryFormService, UserRoleService userService) {
        this.queryFormService = queryFormService;
        this.userService = userService;
    }

    @Autowired
    public void configureGlobal(AuthenticationManagerBuilder auth) throws Exception {
        auth.authenticationProvider(applicationAuthenticationProvider);

        UserRoleConfiguration.getUsers().entrySet()
                .forEach(user -> user.getValue().forEach(
                        throwingConsumerWrapper(u -> auth.inMemoryAuthentication()
                        .withUser(u.getUserName())
                        .password(passwordEncoder().encode(u.getPassword()))
                        .authorities(user.getKey().name()))
                 ));
    }

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http
                .csrf().disable()
                .addFilterBefore(new UserFormPageFilter(queryFormService, userService), BasicAuthenticationFilter.class)
                .addFilterBefore(new LoginPageFilter(), AnonymousAuthenticationFilter.class)
                .authorizeRequests()
                .antMatchers("/static/**").permitAll()
                .antMatchers("/ext/**").permitAll()
                .antMatchers("/userForm/**").permitAll()
                .antMatchers("/api/userForm/**").permitAll()
                .antMatchers("/page/**").hasAuthority(Role.ROLE_USER.name())
                .antMatchers("/admin/**").hasAuthority(Role.ROLE_ADMIN.name())
                .antMatchers("/queryFormList/**").hasAuthority(Role.ROLE_ADMIN.name())
                .antMatchers("/queryFormList").hasAuthority(Role.ROLE_ADMIN.name())
                .antMatchers("/queryFormBuilder/**").hasAuthority(Role.ROLE_ADMIN.name())
                .antMatchers("/login").anonymous()
                .anyRequest().fullyAuthenticated()
                .and()
                .httpBasic()
                .and()
                .formLogin()
                .loginPage("/login")
                .permitAll()
                .successHandler(new LoginRedirectAuthenticationSuccessHandler())
                .defaultSuccessUrl("/queryFormList", false)
                .and()
                .logout()
                .logoutSuccessUrl("/login");
    }

    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }
}