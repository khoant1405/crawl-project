import React from "react";
import Card from "@mui/material/Card";
import Grid from "@mui/material/Grid";
import {Box, styled, Typography} from "@mui/material";
import {Button, ButtonProps, Input} from "reactstrap";
import {purple} from "@mui/material/colors";

const PurpleButton = styled(Button)<ButtonProps>(({ theme }) => ({
    color: theme.palette.getContrastText(purple[500]),
    backgroundColor: purple[500],
    '&:hover': {
        backgroundColor: purple[700],
    },
    border: 'none',
    boxShadow: 'none'
}));

function SignUp() {
    return (
        <Box px={1} width="100%" height="100vh" mx="auto" position="relative" zIndex={2}>
            <Grid container spacing={1} justifyContent="center" alignItems="center" height="100%">
                <Grid item xs={11} sm={9} md={6} lg={5} xl={4}>
                    <Card variant="outlined" sx={{ textAlign: 'center', p: 2 }}>
                        <Typography
                            variant="h6"
                            noWrap
                            sx={{
                                fontFamily: 'monospace',
                                fontWeight: 700,
                                letterSpacing: '.3rem',
                                color: 'inherit',
                                textDecoration: 'none',
                            }}
                        >
                            SIGN UP
                        </Typography>
                        <Box pt={3} pb={3} px={3}>
                            <Box component="form" role="form">
                                <Box mb={2}>
                                    <Input placeholder="User Name" />
                                </Box>
                                <Box mb={2}>
                                    <Input type="password" placeholder="Password" />
                                </Box>
                                <Box mt={3} mb={1}>
                                    <PurpleButton variant="gradient" color="info" sx={{ width: "100%" }}>
                                        Sign up
                                    </PurpleButton>
                                </Box>
                            </Box>
                        </Box>
                    </Card>
                </Grid>
            </Grid>
        </Box>
    );
}

export default SignUp;
