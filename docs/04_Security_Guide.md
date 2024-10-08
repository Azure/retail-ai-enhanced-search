# Security Guide & Best Practices

This document provides guidance on how to secure the solution and best practices to follow.

$${\color{blue} FOR POC}$$

From POC prespective the security considerations are taken care of through the ARM template deployment itself. Calling out the important ones :

> :warning: **Warning: If you are modifying the repo for POC then follow the PROD guidelines**

* [Default Azure Credential](https://learn.microsoft.com/python/api/azure-identity/azure.identity.defaultazurecredential?view=azure-python) : The frontend and backend codes are deployed using Default Azure Credentials from Azure Identity eliminating the need to map or manage individual user/s.

* [RBAC Permission](https://learn.microsoft.com/azure/role-based-access-control/role-assignments-template) : ARM template also takes care of the permissions required for the various components like CosmosDB, Azure Search, Open AI and Static Website to interact with each other without any manual intervention.

* [Private Endpoints for services](https://learn.microsoft.com/azure/private-link/private-endpoint-overview): While the POC does not restrict you but its better to **deploy it as is** so that the communication between these services happen via private endpoint. Explicit approval are required during deployment to enable the same **[Steps 12-13 in Quickstart]**. The private link is created for the outbound connectivity from the Azure AI Search to the Cosmos DB, and Azure AI Search to the Azure Open AI Service. The only public endpoint that gets enabled is for the Static Website **[Step 16 in Quickstart]**.

* [Cleanup for POC](https://learn.microsoft.com/azure/cloud-adoption-framework/scenarios/cloud-scale-analytics/tutorials/cleanup-instructions) : Once tested ensure you leverage the [Cleanup Guide for POC](06_CleanupPOCResources.md) to clean an unwanted resources in your subscription

$${\color{red} FOR PROD}$$

The expectation is customers would have forked the **ai-hub** Public repository for production deployment in their environment.

 ![AI Hub](../media/04_AIHub.PNG)

## Step 1: Security tab of repository

Once in your forked repository go to the **Security** tab.

<img src='../media/04_RepoSecurity.PNG' width='700' height='200'>

## Step 2: Validate security options

There are various defualt options provided in the repository which you can enable. Recommendation is to have all the Security options that you see under security Overview enabled.

<img src='/media/04_SecurityOptions.PNG' width='850' height='400'>

You can also enable alerting in the left hand side panel.

**Dependabot** : Dependabot alerts tell you when your code depends on a package that is insecure. Often, software is built using open-source code packages from a large variety of sources.you may unknowingly be using dependencies that have security flaws, also known as vulnerabilities.Dependabot performs a scan of the default branch of your repository to detect insecure dependencies, and sends Dependabot alerts. 

> :bulb: **For More Information**: [About Dependabot alerts](https://docs.github.com/code-security/dependabot/dependabot-alerts/about-dependabot-alerts#dependabot-alerts-for-vulnerable-dependencies) [About Dependabot auto-triage rules](https://docs.github.com/code-security/dependabot/dependabot-auto-triage-rules/about-dependabot-auto-triage-rules)

**Code-scanning** : Code scanning leverages a third-party tool CodeQL analysis workflow to identify vulnerabilities and errors in the code stored in your repository.Code scanning is available for all public repositories, and for private repositories owned by organizations.By default this is disabled. You manually have to Configure and Enable it.

| ![Configure](/media/04_Configuring.PNG)|![Enable](/media/04_Enabling.PNG) |
| ----- | ------ |

Once you have gone through the [Code Scanning Guide](https://docs.github.com/code-security/code-scanning/enabling-code-scanning/configuring-default-setup-for-code-scanning) you can enable the same by hitting the acknowledgement buttong as shown below

<img src='/media/04_Acknowledge.PNG' width='650' height='300'>

**Secret-scanning** : Secret scanning helps detect and prevent the accidental inclusion of sensitive information such as API keys, passwords, tokens, and other secrets in your repository. It scans your entire Git history on all branches present in your GitHub repository for secrets, even if the repository is archived. GitHub will also periodically run a full Git history scan of existing content in GitHub Advanced Security repositories where secret scanning is enabled.

## Settings Tab -> Security
Additionally, secret scanning scans:

* Descriptions and comments in issues
* Titles, descriptions, and comments, in open and closed historical issues. A notification is sent to the relevant partner when a historical partner pattern is detected.
* Titles, descriptions, and comments in pull requests
* Titles, descriptions, and comments in GitHub Discussions
* Wikis

This additional scanning is free for public repositories.
> :bulb: **For More Information**: [About secret scanning](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/about-secret-scanning) [Customize Secret Scanning](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/about-secret-scanning#customizing-secret-scanning)

All these options can also be found under Settings tab of a Repo
![Setting Tab](../media/04_Settings.PNG)

### Reference Templates

* [GitHub Security Lab](https://securitylab.github.com/)
* [Quickstart for securing your repository](https://docs.github.com/code-security/getting-started/quickstart-for-securing-your-repository)
* [Code Security Main Page](https://docs.github.com/code-security)