import * as React from 'react';
import {Route} from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import FetchData from './components/FetchData';
import './custom.css'
import SignIn from './components/form/SignIn';
import SignUp from './components/form/SignUp';

export default () => (
    <Layout>
        <Route exact path='/' component={Home}/>
        <Route path='/signin' component={SignIn}/>
        <Route path='/signup' component={SignUp}/>
        <Route path='/fetch-data/:startDateIndex?' component={FetchData}/>
    </Layout>
);
