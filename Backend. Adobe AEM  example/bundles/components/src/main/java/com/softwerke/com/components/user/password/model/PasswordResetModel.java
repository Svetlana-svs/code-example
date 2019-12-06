package com.package.name.components.user.password.model;

import com.package.name.core.annotation.Inherited;
import com.package.name.core.model.AbstractComponentModel;
import org.apache.sling.api.SlingHttpServletRequest;
import org.apache.sling.models.annotations.DefaultInjectionStrategy;
import org.apache.sling.models.annotations.Model;
import org.apache.sling.models.annotations.Via;

import javax.inject.Inject;

/**
 * Sling Model for component: /apps/userprofile/components/user/passwordReset
 */
@Model(adaptables = {SlingHttpServletRequest.class}, defaultInjectionStrategy = DefaultInjectionStrategy.OPTIONAL)
public class PasswordResetModel extends AbstractComponentModel {

    public PasswordResetModel(SlingHttpServletRequest request) {
        super(request);
    }

    @Inject
    @Via("resource")
    @Inherited
    private String heading;

    @Inject
    @Via("resource")
    @Inherited
    private String description;

    public String getHeading() {
        return heading;
    }

    public void setHeading(String heading) {
        this.heading = heading;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}
