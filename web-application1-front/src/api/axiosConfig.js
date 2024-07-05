import axios from 'axios';

// Endpoint para obtener la API Key
const getApiKey = async () => {
    try {
        console.log("Getting API key...");
        const response = await axios.get('https://apikey-rotation-mvp-backend.azurewebsites.net/api/apikey/key');
        console.log("API key received:", response.data);
        return response.data;
    } catch (error) {
        console.error("Error getting API key:", error);
        return null;
    }
};

const configureAxios = async () => {
    const apiKey = await getApiKey();
    if (apiKey) {
        axios.defaults.baseURL = 'https://apikey-rotation-mvp-backend.azurewebsites.net';
        axios.defaults.headers.common['X-API-KEY'] = apiKey;
        console.log("Axios configured with API key:", apiKey);
    } else {
        console.error("API key is not available.");
    }
};

export default configureAxios;
