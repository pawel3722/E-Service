import React, { useEffect, useState } from 'react'
import useAxiosPrivate from '../hooks/useAxiosPrivate'
import { NavLink } from 'react-router-dom'
import './UserSideBar.css'

const UserSideBar = () => {
    const [roles, setRoles] = useState([])
    const axiosPrivate = useAxiosPrivate();

    useEffect(() => {
        const getRoles = async () => {
            try {
                const response = await axiosPrivate.get("/api/Auth/users/me")
                console.log(response.data.roles)
                setRoles(response.data.roles)
            }
            catch (err) {
                console.error(err)
            }
        }

        getRoles()
    }, [])


    return (
        <div className='user_side_bar'>
            {
                roles.length > 0
                    ? roles.map((el) => {

                        switch (el.name) {
                            case "Client":
                                return (
                                    <ul>
                                        <li>
                                            <NavLink to='user/home'>Start</NavLink>
                                        </li>
                                        <li>
                                            <NavLink to='user/client-orders'>Moje zamówienia</NavLink>
                                        </li>
                                    </ul>
                                )
                            default:
                                break;
                        }
                    })
                    : <></>
            }

        </div>
    )
}

export default UserSideBar