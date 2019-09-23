<%@page session="false" pageEncoding="utf-8" contentType="text/html; charset=utf-8" trimDirectiveWhitespaces="true" %>
<%@include file="/apps/abbott/global.jsp" %>

<sling:adaptTo var="userProfileComponent" adaptable="${slingRequest}"
        adaptTo="com.abbott.components.user.profile.model.UserProfileModel"/>

<cq:includeClientLib categories="abbott.mask"/>
<cq:includeClientLib categories="abbott.flatpickr"/>


<section class="component-ui user-profile ${userProfileComponent.cssClass}"
         id="userProfileComponent_${userProfileComponent.instanceId}">
    <user-profile
            v-cloak
            :component-settings='${userProfileComponent.componentSettings}'>
    </user-profile>
</section>


<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        Vue.use(VeeValidate);
        new Vue({
            el: '#userProfileComponent_${userProfileComponent.instanceId}'});
    });
</script>
