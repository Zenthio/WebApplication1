import axios from 'axios';

// Endpoint para obtener la API Key
const getApiKey = async () => {
    try {
        const response = await axios.get('https://localhost:7178/api/apikey/key');
        return response.data;
    } catch (error) {
        console.error("Error getting API key:", error);
        return null;
    }
};

// Configurar Axios para usar la API Key en las solicitudes
const configureAxios = async () => {
    const apiKey = await getApiKey();
    if (apiKey) {
        axios.defaults.baseURL = 'https://localhost:7178';
        axios.defaults.headers.common['X-API-KEY'] = apiKey;
    } else {
        console.error("API key is not available.");
    }
};

export default configureAxios;
