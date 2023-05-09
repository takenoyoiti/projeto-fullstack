import React from 'react'
import './App.css';
import {Home} from './Home';
import {Empresa} from './Empresa';
import {Fornecedor} from './Fornecedor';
import { BrowserRouter, Route, Routes, NavLink } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
    <div className="App container">
      <h3 className="d-flex justify-content-center m-3">
        Desafio Fullstack
      </h3>
        
      <nav className="navbar navbar-expand-sm bg-light navbar-dark">
        <ul className="navbar-nav">
          <li className="nav-item- m-1">
            <NavLink className="btn btn-light btn-outline-primary" to="/home">
              Home
            </NavLink>
          </li>
          <li className="nav-item- m-1">
            <NavLink className="btn btn-light btn-outline-primary" to="/empresa">
              Empresa
            </NavLink>
          </li>
          <li className="nav-item- m-1">
            <NavLink className="btn btn-light btn-outline-primary" to="/fornecedor">
            Fornecedor
            </NavLink>
          </li>
        </ul>
      </nav>

      <Routes>
        <Route path='/home' element={<Home/>}/>
        <Route path='/empresa' element={<Empresa/>}/>
        <Route path='/fornecedor' element={<Fornecedor/>}/>
      </Routes>
    </div>
    </BrowserRouter>
  );
}

export default App;
