import React, { useEffect, useState } from 'react'
import { axiosPrivate } from '../api/axios'
import userLogo from '../image/user.png'
import logo from '../image/app_icon.png'
import './NavbarUser.css'

const NavbarUser = () => {

    const [user, setUser] = useState({});

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

    return (
        <nav>
            <img src={logo} alt="Serwis" className='app_logo' />
            <div class='user'>
                <a>{user.name} {user.surname}</a>
                <img src={userLogo} alt='' />
            </div>
        </nav>
    )
}

export default NavbarUser