import { AppBar } from '@material-ui/core';
import React from 'react';
import BarSesion from './Bar/BarSesion';

const AppNavbar = () => {
    return (
        <AppBar position="static" >
            <BarSesion />
       </AppBar>
    );
};

export default AppNavbar;