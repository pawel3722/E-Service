import axios from '../api/axios';
import useAuth from './useAuth';

const useRefreshToken = () => {
    const { setAuth } = useAuth();

    const refresh = async () => {
        const response = await axios.post('/api/Auth/refresh', {}, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
        });
        setAuth(prev => {
            console.log(JSON.stringify(prev));
            console.log(response.data.jwtToken);
            return {
                ...prev,
                //roles: response.data.roles,
                logged: true,
                accessToken: response.data.jwtToken
            }
        });
        return response.data.accessToken;
    }
    return refresh;
};

export default useRefreshToken;