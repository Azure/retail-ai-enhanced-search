# Security Guide & Best Practices

This document provides guidance on how to secure the solution and best practices to follow.

$${\color{blue} FOR POC}$$

From POC prespective the entire 

$${\color{red} FOR PROD}$$

The expectation is customers would have forked the **ai-hub** Public repository for production deployment in their environment.

 ![AI Hub](../media/04_AIHub.PNG)

## Step 1: Security tab of repository

Once in your forked repository go to the **Security** tab 
![Security Tab](../media/04_RepoSecurity.PNG)

## Step 2: Validate Security Options

There are various defualt options provided in the repository which you can enable. Recommendation is to have all the Security options that you see under security Overview enabled.

<img src='/media/04_SecurityOptions.PNG' width='850' height='400'>

You can also enable alerting in the left hand side panel.

Dependabot : Dependabot alerts tell you when your code depends on a package that is insecure. Often, software is built using open-source code packages from a large variety of sources.you may unknowingly be using dependencies that have security flaws, also known as vulnerabilities.Dependabot performs a scan of the default branch of your repository to detect insecure dependencies, and sends Dependabot alerts. 

> :bulb: **For More Information**: [About Dependabot alerts](https://docs.github.com/code-security/dependabot/dependabot-alerts/about-dependabot-alerts#dependabot-alerts-for-vulnerable-dependencies) [About Dependabot auto-triage rules](https://docs.github.com/code-security/dependabot/dependabot-auto-triage-rules/about-dependabot-auto-triage-rules)

Code-scanning : Code scanning leverages a third-party tool CodeQL analysis workflow to identify vulnerabilities and errors in the code stored in your repository.

### Reference Templates

* [GitHub Security Lab](https://securitylab.github.com/)
* [Quickstart for securing your repository](https://docs.github.com/code-security/getting-started/quickstart-for-securing-your-repository)