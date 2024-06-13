import React from 'react'
import './Kontakt.css'
import { Navbar } from '../components/Navbar'
import info from '../image/info.png'
import contact from '../image/contact-mail.png'
import location from '../image/location-pin.png'
import clock from '../image/working-hours.png'
import facebook from '../image/facebook.png'
import linkedin from '../image/linkedin.png'
import twitter from '../image/twitter.png'
import instagram from '../image/instagram.png'



function Kontakt() {
  return (
    <>
      <Navbar />
      <div className='main_kontakt'>
        <section className='dane_firmy'>
          <img src={info} alt='clock' />
          <div>
            <h2>Dane firmy</h2>
            <ul>
              <li>Tech Solutions Sp. z o.o.</li>
              <li>ul. Nowoczesna 10</li>
              <li>00-123 Warszawa</li>
              <li>NIP: 123-456-78-90</li>
              <li>REGON: 987654321</li>
              <li>KRS: 0000123456</li>
            </ul>
          </div>
        </section>

        <section className='kontakt'>
          <img src={contact} alt='contact' />
          <div>
            <h2>Kontakt</h2>
            <ul>
              <li>Główny: +48 22 123 45 67</li>
              <li>Dział Sprzedaży: +48 22 123 45 68</li>
              <li>Wsparcie Techniczne: +48 22 123 45 69</li>
              <li>kontakt@techsolutions.pl</li>
              <li>sprzedaz@techsolutions.pl</li>
              <li>wsparcie@techsolutions.pl</li>
            </ul>
          </div>
        </section>

        <section className='godziny_pracy'>
          <img src={clock} alt='clock' />
          <div>
            <h2>Godziny pracy</h2>
            <ul>
              <li>Poniedziałek - Piątek: 9:00 - 17:00</li>
              <li>Sobota - Niedziela: Zamknięte</li>
            </ul>
          </div>
        </section>
      </div>

      <section className='social_media'>
        <h2>Media społecznościowe</h2>
        <ul>
          <li><img src={facebook} alt='' /></li>
          <li><img src={linkedin} alt='' /></li>
          <li><img src={twitter} alt='' /></li>
          <li><img src={instagram} alt='' /></li>
        </ul>
      </section>

      <div className='lokalizacja'>
        <img src={location} alt='' />
        <iframe
          title={"Lokalizajca firmy"}
          src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2549.160016298861!2d18.67788482468643!3d50.28894082321203!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x471131023ad594ed%3A0x24ca691d97f386a1!2sAkademicka%2016%2C%2044-100%20Gliwice!5e0!3m2!1spl!2spl!4v1716040136777!5m2!1spl!2spl"
          allowFullScreen=""
          loading="lazy"
          referrerPolicy="no-referrer-when-downgrade">
        </iframe>
      </div>
    </>
  )
}

export default Kontakt
