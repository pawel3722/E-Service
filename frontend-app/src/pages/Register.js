import { useRef, useState, useEffect } from "react";
import axios from '../api/axios';
import { Navbar } from "../components/Navbar";
import { useNavigate } from "react-router-dom";

const REGISTER_URL = 'api/Auth/register';

function Register() {
    const userRef = useRef();
    const errRef = useRef();
    const navigate = useNavigate()

    const [name, setName] = useState('')
    const [surname, setSurnName] = useState('')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [confirmPassword, setConfirnPassword] = useState('')

    const [errMsg, setErrMsg] = useState('');

    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        setErrMsg('');
    }, [name, surname, email, password, confirmPassword])

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await axios.post(REGISTER_URL,
                JSON.stringify({ name, surname, email, password, confirmPassword }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            setName('')
            setSurnName('')
            setEmail('')
            setPassword('')
            setConfirnPassword('')
            alert("Rejestracja powiodła się pomyślnie")
        } catch (err) {
            if (!err?.response) {
                setErrMsg('No Server Response');
            } else if (err.response?.status === 409) {
                setErrMsg('Username Taken');
            } else {
                setErrMsg('Registration Failed')
            }
            errRef.current.focus();
        }
    }

    return (
        <>
            <Navbar />
            <section className='content_login'>
                <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
                <h1>Register</h1>
                <form onSubmit={handleSubmit}>
                    <label htmlFor="username">Name:</label>
                    <input
                        type="text"
                        id="username"
                        ref={userRef}
                        autoComplete="off"
                        onChange={(e) => setName(e.target.value)}
                        value={name}
                        required
                    />

                    <label htmlFor="username">Surname:</label>
                    <input
                        type="text"
                        id="username"
                        ref={userRef}
                        autoComplete="off"
                        onChange={(e) => setSurnName(e.target.value)}
                        value={surname}
                        required
                    />

                    <label htmlFor="username">Email:</label>
                    <input
                        type="text"
                        id="username"
                        ref={userRef}
                        autoComplete="off"
                        onChange={(e) => setEmail(e.target.value)}
                        value={email}
                        required
                    />

                    <label htmlFor="password">Password: </label>
                    <input
                        type="password"
                        id="password"
                        onChange={(e) => setPassword(e.target.value)}
                        value={password}
                        required
                    />

                    <label htmlFor="confirm_pwd">Confirm password</label>
                    <input
                        type="password"
                        id="confirm_pwd"
                        onChange={(e) => setConfirnPassword(e.target.value)}
                        value={confirmPassword}
                        required
                    />

                    <button className='logBtn'>Register</button>
                </form>

                <div className='reg'>
                    <button className='regBtn' onClick={() => navigate('/log')}>
                        Sign in
                    </button>
                </div>

            </section>
        </>
    )
}

export default Register