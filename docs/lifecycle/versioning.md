# Product Versioning Guide

We use a unified versioning system across our entire product catalog. This standard framework is designed to be straightforward and intuitive, allowing you to easily monitor functional upgrades, enhancements, and modifications.

Following industry-standard software development practices, our version numbers use a three-digit format:

> **Format:** `V[major].[minor].[maintenance]` (e.g., `V5.0.1`)

*Note: An incremental build number is also appended to identify the precise build execution.*

When an update is released, one of the three digits increases depending on the scope of the changes. For a comprehensive breakdown of any release, please consult the **changelog** included with the update or visit our online documentation.

## Understanding the Digit Hierarchy

| Digit Level | Type | Impact & Scope | Compatibility | Licensing |
| --- | --- | --- | --- | --- |
| **First Digit** | **Major** | Architectural overhauls & structural redesigns. | **Breaking Changes:** May require code rewrites on your application side. | May require a new license or upgrade. |
| **Second Digit** | **Minor** | New features, enhancements, and functional upgrades. | **Compatible:** External APIs remain stable; integration is typically seamless. | Included under existing terms. |
| **Third Digit** | **Maintenance** | Dedicated strictly to bug fixes and stability patches. | **Fully Backward Compatible:** Requires minimal effort (recompilation or file replacement). | Requires a valid [Support Contract](https://technosoftware.com/contact). |

## Detailed Breakdown

### Major Version

An increase in the first digit signifies substantial architectural transformations and core interface redesigns. These releases frequently introduce breaking changes and external API alterations. Transitioning to a new major version (e.g., within an SDK) will likely require updates to your application code. Major releases occur infrequently and may necessitate a license upgrade.

### Minor Version

The second digit represents feature updates, additions, and overall product enhancements. Unlike major updates, minor releases maintain external API compatibility. While internal mechanics might be optimized, the public-facing SDK or API remains stable. In rare instances, minor adjustments might be needed, but existing functionality is preserved and expanded.

### Maintenance Version

The third digit is reserved exclusively for bug fixes and stability resolutions accumulated from customer reports and rigorous testing. These updates introduce zero new features and maintain absolute API compatibility. Deploying a maintenance patch requires minimal effort, usually involving a quick library swap or recompilation. Multiple maintenance updates may be released throughout the year depending on urgency.

**Important**

Requires a valid [Support Contract](https://technosoftware.com/contact).