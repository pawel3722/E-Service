import React, { useEffect, useState } from 'react'
import userLogo from '../image/user.png'
import logo from '../image/app_icon.png'
import './NavbarUser.css'
import useAxiosPrivate from '../hooks/useAxiosPrivate'
import logout from '../image/logout.png'
import useAuth from '../hooks/useAuth'
import { useNavigate, useLocation } from 'react-router-dom'


const NavbarUser = () => {

    const [user, setUser] = useState({})
    const axiosPrivate = useAxiosPrivate()
    const [visible, setVisible] = useState(0)
    const { setAuth } = useAuth()
    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {

        const getName = async () => {
            try {
                const response = await axiosPrivate.get('/api/Auth/users/me')
                //console.log(response.data)
                setUser(response.data)
            }
            catch (err) {
                console.error(err)
            }
        }

        getName()

    }, [])

    const logoutUser = () => {
        setAuth(() => { })
        navigate('/log', { state: { from: location }, replace: true })
    }

    return (
        <>
            <nav>
                <img src={logo} alt="Serwis" className='app_logo' />
                <div class='user'>
                    <a>{user.name} {user.surname}</a>
                    <button onClick={() => setVisible(!visible)}>
                        <img src={userLogo} alt='' />
                    </button>
                </div>
            </nav>
            {
                visible ?
                    <div className='logoutDiv'>
                        <img src={logout} alt='' />
                        <button onClick={logoutUser}>WYLOGUJ SIĘ</button>
                    </div>
                    : <></>

            }

        </>
    )
}

export default NavbarUser