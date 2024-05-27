<p><a target="_blank" href="https://app.eraser.io/workspace/TQPUgzhIiaHNTIvtXemI" id="edit-in-eraser-github-link"><img alt="Edit in Eraser" src="https://firebasestorage.googleapis.com/v0/b/second-petal-295822.appspot.com/o/images%2Fgithub%2FOpen%20in%20Eraser.svg?alt=media&amp;token=968381c8-a7e7-472a-8ed6-4a6626da5501"></a></p>

# React + TypeScript + Vite
This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [﻿@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md)  uses [﻿Babel](https://babeljs.io/)  for Fast Refresh
- [﻿@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc)  uses [﻿SWC](https://swc.rs/)  for Fast Refresh
## Expanding the ESLint configuration
If you are developing a production application, we recommend updating the configuration to enable type aware lint rules:

- Configure the top-level `parserOptions`  property like this:
```js
export default {
  // other rules...
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    project: ['./tsconfig.json', './tsconfig.node.json'],
    tsconfigRootDir: __dirname,
  },
}
```
- Replace `plugin:@typescript-eslint/recommended`  to `plugin:@typescript-eslint/recommended-type-checked`  or `plugin:@typescript-eslint/strict-type-checked` 
- Optionally add `plugin:@typescript-eslint/stylistic-type-checked` 
- Install [﻿eslint-plugin-react](https://github.com/jsx-eslint/eslint-plugin-react)  and add `plugin:react/recommended`  & `plugin:react/jsx-runtime`  to the `extends`  list




<!--- Eraser file: https://app.eraser.io/workspace/TQPUgzhIiaHNTIvtXemI --->