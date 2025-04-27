// main.jsx

import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './src/App.jsx';
import './src/index.css'; 
import { BrowserRouter } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <App />
);
