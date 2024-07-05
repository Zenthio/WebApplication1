import React, { useEffect, useState } from 'react';
import axios from 'axios';
import configureAxios from './api/axiosConfig';
import './App.css'; // Asegúrate de tener este archivo para los estilos

function App() {
    const [secureData, setSecureData] = useState(null);
    const [weatherData, setWeatherData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let isMounted = true; // Guard to check if component is mounted

        const fetchData = async () => {
            console.log("Fetching data...");
            setLoading(true);
            setError(null);

            try {
                await configureAxios();
                console.log("Axios configured.");

                const secureResponse = await axios.get('/secure');
                console.log("Secure data fetched:", secureResponse.data);
                if (isMounted) {
                    setSecureData(secureResponse.data);
                }

                const weatherResponse = await axios.get('/weatherforecast');
                console.log("Weather data fetched:", weatherResponse.data);
                if (isMounted) {
                    setWeatherData(weatherResponse.data);
                }
            } catch (error) {
                console.error("Error fetching data:", error);
                if (isMounted) {
                    setError("Error fetching data");
                }
            } finally {
                if (isMounted) {
                    setLoading(false);
                }
            }
        };

        fetchData();

        return () => {
            isMounted = false; // Cleanup
        };
    }, []); // Empty dependency array to ensure useEffect runs only once

    return (
        <div className="App">
            <header className="App-header">
                <h1>Dashboard</h1>
            </header>
            <main className="App-main">
                {loading && <p>Loading...</p>}
                {error && <p className="error">{error}</p>}
                {secureData && (
                    <div className="data-section">
                        <h2>Secure Data</h2>
                        <p>{secureData}</p>
                    </div>
                )}
                {weatherData && (
                    <div className="data-section">
                        <h2>Weather Forecast</h2>
                        <div className="weather-container">
                            {weatherData.map((forecast, index) => (
                                <div key={index} className="forecast">
                                    <p><strong>Date:</strong> {forecast.date}</p>
                                    <p><strong>Temperature:</strong> {forecast.temperatureC} °C</p>
                                    <p><strong>Summary:</strong> {forecast.summary}</p>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
            </main>
        </div>
    );
}

export default App;
