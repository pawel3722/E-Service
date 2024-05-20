import { useRef, useState, useEffect, useContext } from 'react';
import AuthContext from "../context/AuthProvider";
import { Link, Navigate } from 'react-router-dom'
import './Log.css'
import arrow from '../image/right-arrow.png'

import axios from '../api/axios';

const LOGIN_URL = '/api/Auth/login';

function Log() {

    const { setAuth } = useContext(AuthContext);
    const userRef = useRef();
    const errRef = useRef();

    const [email, setUser] = useState('');
    const [password, setPwd] = useState('');
    const [errMsg, setErrMsg] = useState('');
    const [success, setSuccess] = useState(false);
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
            console.log(JSON.stringify(response?.data));
            //console.log(JSON.stringify(response));
            const roles = response?.data?.roles;
            
            // UWAGA !!!
            // na podstawie tokenu należy wyznaczyć rolę, dlatego po wyświetleniu roles nic nie uzyskujemy
            
            const accessToken = response?.data;

            setAuth({ email, password, roles, accessToken });
            setUser('');
            setPwd('');
            setSuccess(true);
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

    return (
        <>
            {success ? (
                <section>
                    <Navigate to='/admin' />
                    {/* <h1>You are logged in!</h1>
                    <br />
                    <p>
                        <Link to="/home">Go to home</Link>
                    </p> */}
                </section>
            ) : (
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
                        <button>Sign In</button>
                    </form>
                    <p>
                        Need an Account?<br />
                        <br />
                        <span className="line">
                            <Link to='/home' className='register_link'>
                                <img src={arrow} alt=">" />
                                Register
                            </Link>
                        </span>
                    </p>
                </section>
            )}
        </>
    )
}

export default Log
