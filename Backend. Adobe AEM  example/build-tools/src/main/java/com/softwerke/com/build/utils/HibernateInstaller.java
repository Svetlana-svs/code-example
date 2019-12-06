package com.softwerke.com.build.utils;

import org.apache.commons.io.FilenameUtils;
import org.apache.commons.lang3.StringEscapeUtils;
import org.apache.maven.shared.invoker.*;
import org.apache.maven.shared.utils.cli.CommandLineException;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import java.io.*;
import java.lang.reflect.Array;
import java.util.*;

public class HibernateInstaller {

    public static void main(String[] args) throws Exception, MavenInvocationException, CommandLineException {
        String bundleFolder,
                xmlFilePath,
                slingUser,
                slingPassword,
                url;
        if (args != null && args.length == 5) {
            bundleFolder = args[0];
            xmlFilePath = args[1];
            slingUser = args[2];
            slingPassword = args[3];
            url = args[4];
        } else {
            throw new Exception(
                    "Can't execute Hibernate install!\nPlease specify five arguments.");
        }
        final List<String> goals = new ArrayList<String>();

        StringBuilder mvnCmd = new StringBuilder();
        mvnCmd.append("org.apache.sling:sling-maven-plugin:2.4.2:install-file ");
        mvnCmd.append(" -Dsling.user=" + slingUser);
        mvnCmd.append(" -Dsling.password=" + slingPassword);
        mvnCmd.append(" -Dsling.url=" + url);
        mvnCmd.append(" -Dsling.deploy.method=WebConsole ");
        mvnCmd.append(" -Dsling.file=");

        mvnCmd.append(bundleFolder);
        mvnCmd.append("/");

        File file = new File(xmlFilePath);
        DocumentBuilderFactory documentBuilderFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder documentBuilder = documentBuilderFactory.newDocumentBuilder();
        Document document = documentBuilder.parse(file);
        NodeList bundlesList = document.getElementsByTagName("bundle");

        final InvocationRequest invocationRequest = new DefaultInvocationRequest();
//        invocationRequest.setDebug( true );
        final Invoker invoker = new DefaultInvoker();

        for (int i = 0; i < bundlesList.getLength(); i++) {
            String goal = mvnCmd.toString() + bundlesList.item(i).getTextContent();

            installBundle(invocationRequest, invoker, goal);
        }
    }

    private static void installBundle(InvocationRequest invocationRequest, Invoker invoker, String goal)
        throws  MavenInvocationException, CommandLineException {

        invocationRequest.setGoals(Collections.singletonList(goal));
        final InvocationResult invocationResult = invoker.execute(invocationRequest);
        if (invocationResult.getExitCode() != 0) {
            String msg = "Invocation Exception";
            if (invocationResult.getExecutionException() != null) {
                msg = invocationResult.getExecutionException().getMessage();
            }
            throw new CommandLineException(msg);
        }
    }
}
