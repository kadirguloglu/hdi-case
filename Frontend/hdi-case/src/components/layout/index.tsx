import * as React from "react";
import { styled, useTheme, Theme, CSSObject } from "@mui/material/styles";
import Box from "@mui/material/Box";
import MuiDrawer from "@mui/material/Drawer";
import MuiAppBar, { AppBarProps as MuiAppBarProps } from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import CssBaseline from "@mui/material/CssBaseline";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import { Link, Outlet } from "react-router-dom";
import AccountCircle from "@mui/icons-material/AccountCircle";
import MenuItem from "@mui/material/MenuItem";
import Menu from "@mui/material/Menu";
import useAuthentication from "../../contexts/authentication-provider/useAuthentication";
import HomeIcon from "@mui/icons-material/Home";
import { Collapse, TextField } from "@mui/material";
import AbcIcon from "@mui/icons-material/Abc";
import { turkishToEnglish } from "../../utils/string-utils";
import { Enum_Permission } from "../../types/enums/Enum_Permission";
import IsAuthentication from "../../contexts/authentication-provider/IsAuthentication";
import ExpandLess from "@mui/icons-material/ExpandLess";
import ExpandMore from "@mui/icons-material/ExpandMore";
import SettingsIcon from "@mui/icons-material/Settings";
import KeyIcon from "@mui/icons-material/Key";

const drawerWidth = 240;

const openedMixin = (theme: Theme): CSSObject => ({
  width: drawerWidth,
  transition: theme.transitions.create("width", {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: "hidden",
});

const closedMixin = (theme: Theme): CSSObject => ({
  transition: theme.transitions.create("width", {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: "hidden",
  width: `calc(${theme.spacing(7)} + 1px)`,
  [theme.breakpoints.up("sm")]: {
    width: `calc(${theme.spacing(8)} + 1px)`,
  },
});

const DrawerHeader = styled("div")(({ theme }) => ({
  display: "flex",
  alignItems: "center",
  justifyContent: "flex-end",
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
}));

interface AppBarProps extends MuiAppBarProps {
  open?: boolean;
}

const AppBar = styled(MuiAppBar, {
  shouldForwardProp: (prop) => prop !== "open",
})<AppBarProps>(({ theme, open }) => ({
  zIndex: theme.zIndex.drawer + 1,
  transition: theme.transitions.create(["width", "margin"], {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  ...(open && {
    marginLeft: drawerWidth,
    width: `calc(100% - ${drawerWidth}px)`,
    transition: theme.transitions.create(["width", "margin"], {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.enteringScreen,
    }),
  }),
}));

const Drawer = styled(MuiDrawer, {
  shouldForwardProp: (prop) => prop !== "open",
})(({ theme, open }) => ({
  width: drawerWidth,
  flexShrink: 0,
  whiteSpace: "nowrap",
  boxSizing: "border-box",
  ...(open && {
    ...openedMixin(theme),
    "& .MuiDrawer-paper": openedMixin(theme),
  }),
  ...(!open && {
    ...closedMixin(theme),
    "& .MuiDrawer-paper": closedMixin(theme),
  }),
}));

export default function MiniDrawer() {
  const [searchText, setSearchText] = React.useState<string>("");
  const [openNestedMenu, setOpenNestedMenu] = React.useState<
    {
      menuKey: string;
      isOpen: boolean;
    }[]
  >([]);
  const theme = useTheme();
  const [open, setOpen] = React.useState(false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const { signOut } = useAuthentication();

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  const _handleSignOut = () => {
    signOut();
  };

  const SearchMenuItem = ({
    name,
    children,
  }: {
    name: string;
    children: React.ReactNode;
  }) => {
    if (
      turkishToEnglish(name).includes(turkishToEnglish(searchText)) ||
      searchText.length === 0 ||
      !searchText
    )
      return children;
    return null;
  };

  const primaryTypographyProps = {
    fontSize: 14,
  };
  return (
    <Box sx={{ display: "flex" }}>
      <CssBaseline />
      <AppBar position="fixed" open={open}>
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={handleDrawerOpen}
            edge="start"
            sx={{
              marginRight: 5,
              ...(open && { display: "none" }),
            }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
            HDI Administrator
          </Typography>
          <div>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={(event: React.MouseEvent<HTMLElement>) => {
                setAnchorEl(event.currentTarget);
              }}
              color="inherit"
            >
              <AccountCircle />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorEl}
              anchorOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              open={Boolean(anchorEl)}
              onClose={() => {
                setAnchorEl(null);
              }}
            >
              <MenuItem onClick={_handleSignOut}>Sign Out</MenuItem>
            </Menu>
          </div>
        </Toolbar>
      </AppBar>
      <Drawer variant="permanent" open={open}>
        <DrawerHeader>
          <TextField
            placeholder="Search"
            label="Search"
            variant="outlined"
            onChange={(e) => setSearchText(e.target.value)}
          />
          <IconButton onClick={handleDrawerClose}>
            {theme.direction === "rtl" ? (
              <ChevronRightIcon />
            ) : (
              <ChevronLeftIcon />
            )}
          </IconButton>
        </DrawerHeader>
        <Divider />
        <List>
          <SearchMenuItem name="Dashboard" key={searchText}>
            <ListItemButton component={Link} to="/">
              <ListItemIcon>
                <HomeIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText
                primary={"Dashboard"}
                primaryTypographyProps={primaryTypographyProps}
              />
            </ListItemButton>
          </SearchMenuItem>

          <IsAuthentication permission={Enum_Permission.RoleList}>
            <SearchMenuItem name="Roles" key={searchText}>
              <ListItemButton component={Link} to="role">
                <ListItemIcon>
                  <KeyIcon fontSize="small" />
                </ListItemIcon>
                <ListItemText
                  primary={"Roles"}
                  primaryTypographyProps={primaryTypographyProps}
                />
              </ListItemButton>
            </SearchMenuItem>
          </IsAuthentication>

          <IsAuthentication
            permission={[
              Enum_Permission.AdminLoginDataList,
              Enum_Permission.LoggingList,
            ]}
          >
            <ListItemButton
              onClick={() => {
                const currentMenu = openNestedMenu.find(
                  (x) => x.menuKey === "others"
                ) ?? {
                  menuKey: "others",
                  isOpen: false,
                };
                currentMenu.isOpen = !currentMenu.isOpen;
                setOpenNestedMenu([...openNestedMenu, currentMenu]);
              }}
            >
              <ListItemIcon>
                <AbcIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText
                primary="Others"
                primaryTypographyProps={primaryTypographyProps}
              />
              {openNestedMenu.find((x) => x.menuKey === "others")?.isOpen ??
              false ? (
                <ExpandLess />
              ) : (
                <ExpandMore />
              )}
            </ListItemButton>
            <Collapse
              in={
                openNestedMenu.find((x) => x.menuKey === "others")?.isOpen ??
                false
              }
              timeout="auto"
              unmountOnExit
            >
              <List component="div" disablePadding>
                <IsAuthentication
                  permission={Enum_Permission.AdminLoginDataList}
                >
                  <SearchMenuItem name="Admins">
                    <ListItemButton
                      sx={{
                        minHeight: 48,
                        justifyContent: open ? "initial" : "center",
                        px: 2.5,
                        pl: 4,
                      }}
                      component={Link}
                      to="/admin-login-data"
                    >
                      <ListItemIcon
                        sx={{
                          minWidth: 0,
                          mr: open ? 3 : "auto",
                          justifyContent: "center",
                        }}
                      >
                        <SettingsIcon fontSize="small" />
                      </ListItemIcon>
                      <ListItemText
                        primary={"Admins"}
                        sx={{ opacity: open ? 1 : 0 }}
                        primaryTypographyProps={primaryTypographyProps}
                      />
                    </ListItemButton>
                  </SearchMenuItem>
                </IsAuthentication>

                <IsAuthentication permission={Enum_Permission.LoggingList}>
                  <SearchMenuItem name="Logging">
                    <ListItemButton
                      sx={{
                        minHeight: 48,
                        justifyContent: open ? "initial" : "center",
                        px: 2.5,
                        pl: 4,
                      }}
                      component={Link}
                      to="/logging"
                    >
                      <ListItemIcon
                        sx={{
                          minWidth: 0,
                          mr: open ? 3 : "auto",
                          justifyContent: "center",
                        }}
                      >
                        <SettingsIcon fontSize="small" />
                      </ListItemIcon>
                      <ListItemText
                        primary={"Logging"}
                        sx={{ opacity: open ? 1 : 0 }}
                        primaryTypographyProps={primaryTypographyProps}
                      />
                    </ListItemButton>
                  </SearchMenuItem>
                </IsAuthentication>
              </List>
            </Collapse>
          </IsAuthentication>
        </List>
      </Drawer>
      <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
        <DrawerHeader />
        <Outlet />
      </Box>
    </Box>
  );
}
