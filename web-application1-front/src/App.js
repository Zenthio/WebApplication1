/*
import logo from './logo.svg';
import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
*/

// src/App.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import configureAxios from './api/axiosConfig';

function App() {
    const [data, setData] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            await configureAxios();

            try {
                const response = await axios.get('/secure');
                setData(response.data);
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        };

        fetchData();
    }, []);

    return (
        <div className="App">
            <header className="App-header">
                {data ? <p>{data}</p> : <p>Loading...</p>}
            </header>
        </div>
    );
}

export default App;
