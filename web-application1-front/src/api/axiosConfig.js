import axios from 'axios';

let apiKey = null;
let lastUpdated = 0;
const API_KEY_EXPIRATION = 3600 * 1000; // 1 hour

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
    const now = Date.now();

    if (!apiKey || (now - lastUpdated) > API_KEY_EXPIRATION) {
        apiKey = await getApiKey();
        lastUpdated = now;
        if (apiKey) {
            axios.defaults.baseURL = 'https://localhost:7178';
            axios.defaults.headers.common['X-API-KEY'] = apiKey;
        } else {
            console.error("API key is not available.");
        }
    }
};

axios.interceptors.request.use(async config => {
    await configureAxios();
    return config;
});

export default configureAxios;
