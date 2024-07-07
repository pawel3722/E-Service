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
            <ul>
                {
                    roles.length > 0
                        ? roles.map((el) => {

                            switch (el.name) {
                                case "Client":
                                    return (
                                        <>
                                            <li>
                                                <NavLink to='home'>Start</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='client-orders'>Moje zamówienia</NavLink>
                                            </li>
                                        </>
                                    )
                                case "Manager":
                                    return (
                                        <>
                                            <li>
                                                <NavLink to='assign-manager-to-order'>Przypisz menażera</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='assign-services-to-order'>Zarządzaj zamówieniami</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='assign-worker-to-service'>Zarządzaj usługami</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='service-types'>Typy usług</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='models'>Modele</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='parts'>Części</NavLink>
                                            </li>
                                        </>
                                    )
                                case "Seller":
                                    return (
                                        <>
                                            <li>
                                                <NavLink to='new-order'>Nowe zamówienie</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='pay-for-order'>Opłacenie zamówienia</NavLink>
                                            </li>
                                            <li>
                                                <NavLink to='deliver-order'>Odbiór zamówienia</NavLink>
                                            </li>
                                        </>
                                    )
                                case "Serviceman":
                                    return (
                                        <>
                                            <li>
                                                <NavLink to='serviceman-services'>Moje usługi</NavLink>
                                            </li>
                                        </>
                                    )
                                default:
                                    break;
                            }
                        })
                        : <></>
                }
            </ul>
        </div>
    )
}

export default UserSideBar