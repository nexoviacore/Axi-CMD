# 03 — Permission Scoping & Cache Invalidation

**What to build:**
Restrict "Create New" SDK suggestions to users with developer build access (`buildAccess`), and update script version string `axicmdmain.js?v=185` in `AxiCMDMainPage.html`.

**Blocked by:** #171

**Status:** ready-for-agent

- [ ] Enforce `buildAccess` permission check before prepending "Create New" suggestions.
- [ ] Update `axicmdmain.js?v=185` in `AxiCMDMainPage.html` for cache invalidation.
- [ ] Perform manual verification in browser environment.
