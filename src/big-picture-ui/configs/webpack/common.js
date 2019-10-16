// shared config (dev and prod)
const {resolve} = require('path');
const merge = require('webpack-merge');
const WebpackCommonConfig = require('@wface/container/src/configs/webpack/common');
const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

module.exports = merge(WebpackCommonConfig, {  
  name: 'BigPicture',
  context: resolve(__dirname, '../../'),
  plugins: [
    new MonacoWebpackPlugin({
      // available options are documented at https://github.com/Microsoft/monaco-editor-webpack-plugin#options
      languages: ['csharp', 'javascript', 'json']
    })
  ]
});