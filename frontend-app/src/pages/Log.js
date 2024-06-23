import { useRef, useState, useEffect } from 'react';
import useAuth from '../hooks/useAuth';
import { useNavigate } from 'react-router-dom'
import './Log.css'

import axios from '../api/axios';
import { Navbar } from '../components/Navbar';

const LOGIN_URL = '/api/Auth/login';

function Log() {

    const { setAuth, persist, setPersist } = useAuth();

    const navigate = useNavigate()

    const userRef = useRef();
    const errRef = useRef();

    const [email, setUser] = useState('');
    const [password, setPwd] = useState('');
    const [errMsg, setErrMsg] = useState('');
    //const [token, setToken] = useState('');

    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        setErrMsg('');
    }, [email, password])

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(LOGIN_URL,
                JSON.stringify({ email, password }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            // console.log(JSON.stringify(response?.data));
            // console.log(JSON.stringify(response));
            // console.log(response?.data);

            // UWAGA !!!
            // na podstawie tokenu należy wyznaczyć rolę, dlatego po wyświetleniu roles nic nie uzyskujemy

            const accessToken = response?.data?.jwtToken;

            setAuth({ email, password, accessToken, logged: true });
            setUser('');
            setPwd('');
            navigate('/user')
        } catch (err) {
            if (!err?.response) {
                setErrMsg('No Server Response');
            } else if (err.response?.status === 400) {
                setErrMsg('Missing Username or Password');
            } else if (err.response?.status === 401) {
                setErrMsg('Unauthorized');
            } else {
                setErrMsg('Login Failed');
            }
            errRef.current.focus();
        }
    }

    const togglePersist = () => {
        setPersist(prev => !prev);
    }

    useEffect(() => {
        localStorage.setItem("persist", persist);
    }, [persist])

    return (
        <>
            <Navbar />
            <section className='content_login'>
                <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
                <h1>Sign In</h1>
                <form onSubmit={handleSubmit}>

                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        ref={userRef}
                        autoComplete="off"
                        onChange={(e) => setUser(e.target.value)}
                        value={email}
                        required
                    />

                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        onChange={(e) => setPwd(e.target.value)}
                        value={password}
                        required
                    />

                    <div className="persistCheck">
                        <input
                            type="checkbox"
                            id="persist"
                            onChange={togglePersist}
                            checked={persist}
                        />
                        <label htmlFor="persist">Trust This Device</label>
                    </div>

                    <button className='logBtn'>
                        Sign In
                    </button>

                </form>

                <div className='reg'>
                    <button className='regBtn' onClick={() => navigate('/home')}>
                        Register
                    </button>
                </div>

            </section>
        </>
    )
}

export default Log
