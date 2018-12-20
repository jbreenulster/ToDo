const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
  entry: './ClientApp/main.ts',
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: ['ts-loader', 'angular2-template-loader'],
        exclude: /node_modules/
      },
      {
        test: /\.(html|css)$/,
        loader: 'raw-loader'
      },
    ]
  },
  resolve: {
    extensions: ['.ts', '.js']
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: './ClientApp/index.html',
      filename: 'index.html',
      inject: 'body'
    }),
    new webpack.DefinePlugin({
      // global app config object
      config: JSON.stringify({
        apiUrl: 'http://localhost:8840'
      })
    })
  ],
  optimization: {
    splitChunks: {
      chunks: 'all',
    },
    runtimeChunk: true
  },
  devServer: {
      historyApiFallback: true,
      proxy: {
          "*": "http://[::1]:9183"
          // "secure": false,
          // "changeOrigin": true
      }
  }
};