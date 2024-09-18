import { useState } from "react";
import useAuthentication from "../../contexts/authentication-provider/useAuthentication";
import { Box, TextField } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { toast } from "react-toastify";
import Grid from "@mui/material/Grid2";

function App() {
  const { signIn } = useAuthentication();
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const handleClickLogin = async () => {
    if (email.length == 0) {
      toast("Email required", {
        type: "error",
      });
      return;
    }
    if (password.length == 0) {
      toast("Password required", {
        type: "error",
      });
      return;
    }
    setIsLoading(true);
    const result = await signIn(email, password);
    if (!result) {
      setIsLoading(false);
      toast("Error", {
        type: "error",
      });
      setEmail("");
      setPassword("");
    }
  };
  return (
    <Grid
      container
      justifyContent={"center"}
      alignItems={"center"}
      height={"100vh"}
    >
      <Grid size={3}>
        <Box
          display={"flex"}
          flexDirection={"column"}
          border={1}
          padding={4}
          borderColor={"#dedede"}
          borderRadius={4}
        >
          <img
            style={{
              marginTop: 4,
              marginBottom: 4,
              height: 68,
              objectFit: "contain",
            }}
            src={"logo.png"}
          />
          <TextField
            type="email"
            placeholder="E-mail"
            variant="outlined"
            value={email}
            onChange={(e) => {
              setEmail(e.target.value);
            }}
            style={{
              marginTop: 4,
            }}
          />
          <TextField
            type="password"
            placeholder="Password"
            value={password}
            variant="outlined"
            onChange={(e) => {
              setPassword(e.target.value);
            }}
            style={{
              marginTop: 4,
            }}
          />
          <LoadingButton
            loading={isLoading}
            variant="outlined"
            disabled={isLoading}
            onClick={handleClickLogin}
            style={{
              marginTop: 4,
            }}
            loadingPosition="start"
          >
            Login
          </LoadingButton>
        </Box>
      </Grid>
    </Grid>
  );
}

export default App;
