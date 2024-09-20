/* eslint-disable @typescript-eslint/no-explicit-any */
import { useRef } from "react";
import {
  DataGrid,
  Column,
  FilterRow,
  FilterPanel,
  HeaderFilter,
  Pager,
  Paging,
  RemoteOperations,
  Grouping,
  Export,
  ColumnChooser,
  GroupPanel,
  SearchPanel,
  Editing,
  Button,
  DataGridRef,
} from "devextreme-react/data-grid";
import { Link, useNavigate } from "react-router-dom";
import { Breadcrumbs } from "@mui/material";
import Grid from "@mui/material/Grid2";

const RoleScreen = () => {
  const navigate = useNavigate();
  const dataGridRef = useRef<DataGridRef>(null);

  const openRoleDetailPage = (data: any) => {
    navigate(`/editRole/${data.row.data.id}`);
  };

  return (
    <Grid container spacing={2}>
      <Grid size={12}>
        <Breadcrumbs>
          <Link to="/">Dashboard</Link>
        </Breadcrumbs>
      </Grid>
      <Grid size={12}>
        <DataGrid
          ref={dataGridRef}
          showBorders={true}
          hoverStateEnabled={true}
          selection={{ mode: "single" }}
          dataSource={{
            store: {
              type: "odata" as const,
              url: `${import.meta.env.VITE_API_ODATA_URL}/api/odata/v1/role`,
              version: 4,
              key: "id",
              keyType: "Integer",
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
            dataField="id"
            allowEditing={false}
            width={100}
            caption={"#"}
            dataType={"string"}
            visible={false}
          />
          <Column
            dataField={"name"}
            caption={"Name"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"description"}
            caption={"Description"}
            dataType={"string"}
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
          <Paging enabled={true} defaultPageSize={20} defaultPageIndex={0} />
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
            allowDeleting={true}
            allowAdding={true}
          />
          <Column type="buttons" width={110}>
            <Button name="edit" />
            <Button
              hint="Clone"
              icon="detailslayout"
              onClick={openRoleDetailPage}
            />
            <Button name="delete" />
          </Column>
        </DataGrid>
      </Grid>
    </Grid>
  );
};

RoleScreen.propTypes = {};

export default RoleScreen;
