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
} from "devextreme-react/data-grid";
import "devextreme/data/odata/store";
import "devextreme-react/text-area";
import { Breadcrumbs } from "@mui/material";
import Grid from "@mui/material/Grid2";
import { Link } from "react-router-dom";

const LoggingScreen = () => {
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
              url: `${import.meta.env.VITE_API_ODATA_URL}/api/odata/v1/Logging`,
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
            sort: [{ selector: "createdDate", desc: true }],
          }}
        >
          <Column
            dataField="id"
            allowEditing={false}
            caption={"#"}
            dataType={"string"}
            visible={false}
          />
          <Column
            dataField={"userId"}
            caption={"user_id"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"tableName"}
            caption={"table_name"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"tableId"}
            caption={"table_id"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"operationType"}
            caption={"operation_type"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"ipAddress"}
            caption={"ip_address"}
            dataType={"string"}
            validationRules={[
              {
                type: "required",
              },
            ]}
          />
          <Column
            dataField={"createdDate"}
            caption={"createdDate"}
            dataType={"datetime"}
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
          <Paging enabled={true} defaultPageSize={100} />
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
        </DataGrid>
      </Grid>
    </Grid>
  );
};

export default LoggingScreen;
