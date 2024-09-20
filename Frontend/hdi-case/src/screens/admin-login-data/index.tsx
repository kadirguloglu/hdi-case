import {
  DataGrid,
  Column,
  FilterRow,
  FilterPanel,
  Pager,
  Grouping,
  HeaderFilter,
  Paging,
  RemoteOperations,
  Export,
  ColumnChooser,
  GroupPanel,
  SearchPanel,
  Editing,
  Button,
} from "devextreme-react/data-grid";
import "devextreme/data/odata/store";
import "devextreme-react/text-area";
import { Breadcrumbs } from "@mui/material";
import Grid from "@mui/material/Grid2";
import { Link } from "react-router-dom";
import useAuthentication from "../../contexts/authentication-provider/useAuthentication";
import { Enum_Permission } from "../../types/enums/Enum_Permission";

const AdminLoginDataScreen = () => {
  const { isAuthenticate } = useAuthentication();
  return (
    <Grid container spacing={2}>
      <Grid size={12}>
        <Breadcrumbs>
          <Link to="/">Dashboard</Link>
        </Breadcrumbs>
      </Grid>
      <Grid size={12}>
        <DataGrid
          showBorders={true}
          hoverStateEnabled={true}
          selection={{ mode: "single" }}
          dataSource={{
            store: {
              type: "odata" as const,
              url: `${
                import.meta.env.VITE_API_ODATA_URL
              }/api/odata/v1/AdminLoginData`,
              version: 4,
              key: "id",
              keyType: "Integer",
              // eslint-disable-next-line @typescript-eslint/no-explicit-any
              beforeSend: (e: any) => {
                e.headers = {
                  Authorization: `Bearer ${localStorage.getItem(
                    "bearerToken"
                  )}`,
                };
              },
            },
          }}
        >
          <Column
            dataField="_Id"
            allowEditing={false}
            caption={"#"}
            dataType={"string"}
            visible={false}
          />
          <Column
            dataField={"email"}
            caption={"Email"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"password"}
            caption={"Password"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <FilterRow visible={true} />
          <FilterPanel visible={true} />
          <HeaderFilter visible={true} />
          <Pager
            showPageSizeSelector={true}
            allowedPageSizes={[20, 40, 60, 80, 100]}
            showNavigationButtons={true}
            showInfo={true}
          />
          <Paging enabled={true} defaultPageSize={20} />
          <RemoteOperations
            filtering={true}
            paging={true}
            sorting={true}
            summary={true}
            grouping={true}
            groupPaging={true}
          />
          <Grouping />
          <Export enabled={true} />
          <ColumnChooser enabled={true} mode="select" />
          <GroupPanel visible={true} />
          <SearchPanel visible={true} />
          <Editing
            mode="popup"
            allowUpdating={true}
            allowAdding={isAuthenticate(Enum_Permission.AdminLoginDataList)}
            allowDeleting={true}
          />
          <Column type="buttons" width={110}>
            {isAuthenticate(Enum_Permission.AdminLoginDataUpdate) && (
              <Button name="edit" />
            )}
            {isAuthenticate(Enum_Permission.AdminLoginDataDelete) && (
              <Button name="delete" />
            )}
          </Column>
        </DataGrid>
      </Grid>
    </Grid>
  );
};

export default AdminLoginDataScreen;
