import React, { memo } from "react";

interface TableBodyType {
  loading: boolean;
  tableColumns: { [key: string]: unknown };
  tableData: { [key: string]: unknown }[];
  getData: (params: { [key: string]: unknown }) => Promise<void>;
  deActivateData: (params: { [key: string]: unknown }) => Promise<void>;
}

const TableBody = memo((props: TableBodyType): React.JSX.Element => {
  const columnLength: number = (Object.entries(props.tableColumns).length + 2);

  return (
    <tbody>
      {props.loading
        ? (
            <tr>
              <td className="text-center" colSpan={columnLength}>Loading...</td>
            </tr>
          )
        : (props.tableData.length
            ? (
                props.tableData.map((rowData: { [key: string]: unknown }, i: number) => (
                  <tr key={i}>
                    {Object.entries(props.tableColumns).map(([column, callback], j: number) => (
                      <td key={`${i}${j}`}>
                        {typeof callback === "function"
                          ? callback(rowData[column])
                          : rowData[column] as string}
                      </td>
                    ))}
                    <td className="text-center">
                      <span className={`badge bg-${ rowData.isActive ? `success` : `danger` }`}>{ rowData.isActive ? `Active` : `Inactive` }</span>
                    </td>
                    <td className="text-end">
                      <div className="dropdown">
                        <button
                          aria-expanded="false"
                          className="btn btn-default btn-sm"
                          id={`dropdownMenuButton${i}`}
                          type="button"
                          data-bs-toggle="dropdown"     
                        ><i className="bi bi-three-dots-vertical"></i></button>
                        <div className="dropdown-menu dropdown-menu-left m-0 p-0" aria-labelledby={`dropdownMenuButton${i}`}>
                          <h6 className="dropdown-header">Options</h6>
                          {/* View button */}
                          <button
                            className="btn btn-info btn-sm dropdown-item text-info"
                            onClick={() => props.getData({ id: rowData.id, editable: false })}
                            type="button"
                          ><i className="bi bi-info-circle-fill"></i><span>View</span></button>
                          {/* Edit button */}
                          <button
                            className="btn btn-success btn-sm dropdown-item text-success"
                            onClick={() => props.getData({ id: rowData.id, editable: true })}
                            type="button"
                          ><i className="bi bi-pencil-square"></i><span>Edit</span></button>
                          <hr className="dropdown-divider" />
                          {rowData.isActive 
                            ? (// Deactivate button
                                <button
                                    className="btn btn-danger btn-sm dropdown-item text-danger"
                                    onClick={() => props.deActivateData({ id: rowData.id, active: true })}
                                    type="button"
                                    // data-toggle="popover"
                                    // title=""
                                ><i className="bi bi-trash3-fill"></i><span>Delete</span></button>
                              )
                            : (// Activate button
                                <button
                                    className="btn btn-success btn-sm dropdown-item text-success"
                                    onClick={() => props.deActivateData({ id: rowData.id, active: false })}
                                    type="button"
                                ><i className="bi bi-recycle"></i><span>Activate</span></button>
                              )
                          }
                        </div>
                      </div>
                    </td>
                  </tr>
                ))
              )
            : (
                <tr>
                  <td className="text-center" colSpan={columnLength}>No available data.</td>
                </tr>
              )
          )
      }
    </tbody>
  );
});

export default TableBody;