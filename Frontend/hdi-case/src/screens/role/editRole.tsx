/* eslint-disable @typescript-eslint/no-explicit-any */
import { useState, useEffect } from "react";
import {
  Breadcrumbs,
  Checkbox,
  FormControlLabel,
  Skeleton,
  Stack,
  Tab,
  Tabs,
  TextField,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
import { Link, useNavigate, useParams } from "react-router-dom";
import PermissionCategoryList from "../../types/dto/permissionDto/permissionCategoryList";
import {
  getPermissions,
  updateRole,
  getRoleById,
} from "../../business/actions/role";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import PermissionList from "../../types/dto/permissionDto/permissionList";
import { useForm, Controller } from "react-hook-form";
import { TagBox } from "devextreme-react";
import RoleDto from "../../types/dto/roleDto/roleDto";
import { LoadingButton } from "@mui/lab";
import SaveIcon from "@mui/icons-material/Save";
import Swal from "sweetalert2";

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;
  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

function a11yProps(index: number) {
  return {
    id: `simple-tab-${index}`,
    "aria-controls": `simple-tabpanel-${index}`,
  };
}

const EditRoleScreen = () => {
  const {
    control,
    register,
    setValue,
    handleSubmit,
    formState: { errors },
    getValues,
  } = useForm({
    defaultValues: {
      id: "",
      Name: "",
      Description: "",
      Admins: [] as Array<number>,
      PermissionKeys: [] as Array<number>,
    },
  });
  const navigate = useNavigate();
  const { id } = useParams();
  const [permissionCategoryActiveTab, setPermissionCategoryActiveTab] =
    useState<number>(0);
  const [permissions, setPermission] = useState<Array<PermissionCategoryList>>(
    []
  );
  const [checkedPermissions, setCheckedPermission] = useState<Array<number>>(
    []
  );
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isSaveLoading, setIsSaveLoading] = useState<boolean>(false);

  useEffect(() => {
    async function initRoleDetailAndPermissions() {
      if (id) {
        setIsLoading(true);
        const roleResult = await getRoleById(Number(id));
        if (roleResult.data.isSuccessfull) {
          if (roleResult.data.data?.id) setValue("id", roleResult.data.data.id);
          if (roleResult.data.data?.name)
            setValue("Name", roleResult.data.data.name);
          if (roleResult.data.data?.description)
            setValue("Description", roleResult.data.data.description);
          if (roleResult.data.data?.admins)
            setValue("Admins", roleResult.data.data.admins);
          if (roleResult.data.data?.permissionKeys)
            setCheckedPermission(roleResult.data.data.permissionKeys);
        }
      }
      const result = await getPermissions();
      setIsLoading(false);
      if (result && result.data) setPermission(result.data);
    }
    initRoleDetailAndPermissions();
  }, []);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setPermissionCategoryActiveTab(newValue);
  };

  const renderChildPermissions = (
    childPermissions: Array<PermissionList>,
    parentPermission: PermissionList,
    permissionCategory: PermissionCategoryList
  ) => {
    return childPermissions.map((childPermission: PermissionList) => {
      return (
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            ml: 3,
          }}
        >
          <FormControlLabel
            label={childPermission.value}
            control={
              <Checkbox
                checked={checkedPermissions.some(
                  (x) => x == childPermission.key
                )}
                disabled={
                  !checkedPermissions.some((x) => x == parentPermission.key)
                }
                onChange={(event, result) => {
                  onChangePermission(
                    permissionCategory,
                    childPermission.key,
                    result
                  );
                }}
              />
            }
          />
          {childPermission.childPermission &&
            renderChildPermissions(
              childPermission.childPermission,
              childPermission,
              permissionCategory
            )}
        </Box>
      );
    });
  };

  const onChangePermission = (
    permissionCategory: PermissionCategoryList,
    key: number,
    isChecked: boolean
  ) => {
    if (isChecked) {
      setCheckedPermission([...checkedPermissions, key]);
    } else {
      const filteredPermissions = getChildPermission(
        permissionCategory,
        key,
        null
      );
      const resultPermissions = checkedPermissions.filter(
        (x) => !filteredPermissions.some((y) => y == x)
      );
      setCheckedPermission(resultPermissions);
    }
  };

  const getChildPermission = (
    permissionCategory: PermissionCategoryList,
    parentPermission: number,
    childPermissions: Array<PermissionList> | null
  ) => {
    const filteredChildPermissions: Array<number> = [];
    const findedPermission = permissions.find(
      (x) => x.categoryKey == permissionCategory.categoryKey
    );
    if (findedPermission && findedPermission.permissions) {
      const findedPermission2 = findedPermission.permissions.find(
        (x) => x.key == parentPermission
      );
      if (findedPermission2) {
        filteredChildPermissions.push(findedPermission2.key);
        if (findedPermission2.childPermission) {
          filteredChildPermissions.push(
            ...findedPermission2.childPermission.map((t) => {
              return t.key;
            })
          );
          for (let i = 0; i < findedPermission2.childPermission?.length; i++) {
            const inChildPermissions = getChildPermission(
              permissionCategory,
              findedPermission2.childPermission[i].key,
              findedPermission2.childPermission
            );
            if (inChildPermissions)
              filteredChildPermissions.push(...inChildPermissions);
          }
        }
      } else if (childPermissions) {
        const findedPermission2 = childPermissions.find(
          (x) => x.key == parentPermission
        );
        if (findedPermission2) {
          filteredChildPermissions.push(findedPermission2.key);
          if (findedPermission2.childPermission) {
            filteredChildPermissions.push(
              ...findedPermission2.childPermission.map((t) => {
                return t.key;
              })
            );
            for (
              let i = 0;
              i < findedPermission2.childPermission?.length;
              i++
            ) {
              const inChildPermissions = getChildPermission(
                permissionCategory,
                findedPermission2.childPermission[i].key,
                findedPermission2.childPermission
              );
              if (inChildPermissions)
                filteredChildPermissions.push(...inChildPermissions);
            }
          }
        }
      } else {
        for (let i = 0; i < findedPermission.permissions.length; i++) {
          const element = findedPermission.permissions[i];
          if (element.childPermission) {
            const findedPermission3 = element.childPermission.find(
              (x) => x.key == parentPermission
            );
            if (findedPermission3) {
              const inChildPermission = getChildPermission(
                permissionCategory,
                parentPermission,
                element.childPermission
              );
              if (inChildPermission) {
                filteredChildPermissions.push(...inChildPermission);
              }
            } else {
              const findedPermission2 = getParentPermission(
                parentPermission,
                element.childPermission
              );
              if (findedPermission2) {
                const inChildPermissions = getChildPermission(
                  permissionCategory,
                  parentPermission,
                  findedPermission2.childPermission
                );
                if (inChildPermissions)
                  filteredChildPermissions.push(...inChildPermissions);
              }
            }
          }
        }
      }
    }
    return filteredChildPermissions;
  };

  const getParentPermission = (
    parentPermission: number,
    childPermissions: Array<PermissionList>
  ): PermissionList | undefined => {
    let findedPermission: PermissionList | undefined = childPermissions.find(
      (x) => x.key == parentPermission
    );
    if (findedPermission) return findedPermission;
    else {
      for (let i = 0; i < childPermissions.length; i++) {
        const element = childPermissions[i];
        if (element.childPermission) {
          findedPermission = element.childPermission.find(
            (x) => x.key == parentPermission
          );
          if (findedPermission) return element;
          else {
            findedPermission = getParentPermission(
              parentPermission,
              element.childPermission
            );
            if (findedPermission) return findedPermission;
          }
        }
      }
    }
  };

  const saveRole = async (e: any) => {
    const dto: RoleDto = {
      id: e.id,
      name: e.Name,
      description: e.Description,
      admins: e.Admins,
      permissionKeys: checkedPermissions,
    };
    setIsSaveLoading(true);
    const result = await updateRole(dto);
    setIsSaveLoading(false);
    if (result.data.isSuccessfull) {
      Swal.fire({
        title: "Success",
        text: "Role updated",
        icon: "success",
        timer: 3000,
        timerProgressBar: true,
        willClose: () => {
          navigate("/role");
        },
      });
    } else {
      Swal.fire({
        title: "Error",
        text: result.data.message ?? "Something went wrong",
        icon: "error",
      });
    }
  };

  return (
    <Grid container spacing={6}>
      <Grid size={12}>
        <Breadcrumbs>
          <Link to="/">Dashboard</Link>
          <Link to="/role">Roles</Link>
        </Breadcrumbs>
      </Grid>
      <Grid size={12}>
        <form onSubmit={handleSubmit(saveRole)}>
          <Stack direction="row" spacing={2}>
            <Grid size={3}>
              <Grid size={12}>
                {isLoading ? (
                  <Skeleton variant="rectangular" width={620} height={40} />
                ) : (
                  <TextField
                    label="Role Name"
                    variant="standard"
                    fullWidth={true}
                    {...register("Name", {
                      required: "This is required",
                    })}
                  />
                )}
                {errors.Name && (
                  <span style={{ color: "red" }}>{errors.Name.message}</span>
                )}
              </Grid>
              <Grid size={12} mt={2}>
                {isLoading ? (
                  <Skeleton variant="rectangular" width={620} height={150} />
                ) : (
                  <TextField
                    label="Description"
                    multiline
                    rows={4}
                    variant="standard"
                    fullWidth={true}
                    {...register("Description")}
                  />
                )}
              </Grid>
              <Grid size={12} mt={3}>
                <Controller
                  control={control}
                  {...register("Admins")}
                  render={() =>
                    isLoading ? (
                      <Skeleton variant="rectangular" width={620} height={40} />
                    ) : (
                      <TagBox
                        dataSource={{
                          store: {
                            type: "odata",
                            url: `${
                              import.meta.env.VITE_API_ODATA_URL
                            }/v1/adminLoginData`,
                            key: "id",
                            keyType: "Integer",
                            version: 4,
                            beforeSend: (e: any) => {
                              e.headers = {
                                Authorization: `Bearer ${localStorage.getItem(
                                  "bearerToken"
                                )}`,
                              };
                            },
                          },
                        }}
                        displayExpr="email"
                        valueExpr="id"
                        onSelectionChanged={(e) => {
                          if (e.removedItems && e.removedItems.length > 0) {
                            setValue(
                              "Admins",
                              getValues("Admins").filter(
                                (x) => !e.removedItems.some((y) => y.id == x)
                              )
                            );
                          }
                        }}
                        onItemClick={(e) => {
                          setValue("Admins", [
                            ...getValues("Admins"),
                            e.itemData.id,
                          ]);
                        }}
                        defaultValue={getValues("Admins")}
                      />
                    )
                  }
                />
              </Grid>
            </Grid>
            <Grid size={9}>
              <Grid size={12}>
                {isLoading ? (
                  <Skeleton variant="rectangular" width={999} height={270} />
                ) : (
                  <>
                    <Tabs
                      variant="scrollable"
                      scrollButtons="auto"
                      value={permissionCategoryActiveTab}
                      onChange={handleTabChange}
                    >
                      {permissions.map(
                        (item: PermissionCategoryList, index) => {
                          return (
                            <Tab
                              key={index}
                              label={item.categoryName}
                              {...a11yProps(index)}
                            />
                          );
                        }
                      )}
                    </Tabs>
                    {permissions.map((item: PermissionCategoryList, index) => {
                      return (
                        <TabPanel
                          key={index}
                          value={permissionCategoryActiveTab}
                          index={index}
                        >
                          {item.permissions?.map(
                            (permission: PermissionList) => {
                              return (
                                <div key={permission.key}>
                                  <FormControlLabel
                                    label={permission.value}
                                    control={
                                      <Checkbox
                                        checked={checkedPermissions.some(
                                          (x) => x == permission.key
                                        )}
                                        onChange={(event, result) => {
                                          onChangePermission(
                                            item,
                                            permission.key,
                                            result
                                          );
                                        }}
                                      />
                                    }
                                  />
                                  {permission.childPermission &&
                                    renderChildPermissions(
                                      permission.childPermission,
                                      permission,
                                      item
                                    )}
                                </div>
                              );
                            }
                          )}
                        </TabPanel>
                      );
                    })}
                  </>
                )}
              </Grid>
              <Grid size={12}>
                {isLoading ? (
                  <></>
                ) : (
                  <LoadingButton
                    onClick={handleSubmit(saveRole)}
                    loading={isSaveLoading}
                    loadingPosition="start"
                    startIcon={<SaveIcon />}
                    variant="outlined"
                  >
                    <span>Save</span>
                  </LoadingButton>
                )}
              </Grid>
            </Grid>
          </Stack>
        </form>
      </Grid>
    </Grid>
  );
};

EditRoleScreen.propTypes = {};

export default EditRoleScreen;
