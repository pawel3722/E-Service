import React from 'react'
import { Link } from 'react-router-dom'
import './Navbar.css'
import logo from '../image/app_icon.png'

export const Navbar = () => {
  return (
    <nav>
        <img src={logo} alt="Serwis" className='app_logo'/>
        <ul>
            <li><Link to="/home" className='options'>Home</Link></li>
            <li><Link to="/uslugi" className='options'>Usługi</Link></li>
            <li><Link to="/kontakt" className='options'>Kontakt</Link></li> 
        </ul>
        <Link to="/log" className='log_link'>Log in</Link>
    </nav>
  )
}
