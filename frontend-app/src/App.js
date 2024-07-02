import { Route, Routes } from 'react-router-dom';
import './App.css';
import Uslugi from './pages/Uslugi';
import Kontakt from './pages/Kontakt';
import Log from './pages/Log';
import Home from './pages/Home';
import UserPage from './pages/UserPage';
import ClientOrders from './pages/client/ClientOrders';
import ProtectedRoutes from './context/ProtectedRoutes';
import PersistLogin from './components/PersistLogin';
import HomePageWidget from './components/HomePageWidget';
import NewOrder from './pages/seller/NewOrder'
import DeliverOrder from './pages/seller/DeliverOrder'
import PayForOrder from './pages/seller/PayForOrder';

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="home" element={<Home />} />
        <Route path="uslugi" element={<Uslugi />} />
        <Route path="kontakt" element={<Kontakt />} />
        <Route path="log" element={<Log />} />

        <Route element={<PersistLogin />}>
          <Route element={<ProtectedRoutes />} >
            <Route path='user' element={<UserPage />} >
              <Route path='home' element={<HomePageWidget />} />
              <Route path='client-orders' element={<ClientOrders />} />
              <Route path='new-order' element={<NewOrder />} />
              <Route path='deliver-order' element={<DeliverOrder />} />
              <Route path='pay-for-order' element={<PayForOrder />} />
            </Route>
          </Route>
        </Route>

        {/* <Route element={<PersistLogin />}>
          <Route element={<ProtectedRoutes />} >
          <Route path='client-orders' element={<ClientOrders />} />
          </Route>
        </Route> */}
      </Routes>
    </div>
  );
}

export default App;
