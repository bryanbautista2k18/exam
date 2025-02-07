import React, { memo } from "react";

interface TableLimiterType {
  pageLimit: number;
  changePageLimit: (limit: number) => void;
}

const TableLimiter = memo((props: TableLimiterType): React.JSX.Element => {
  return (
    <div className="d-flex align-items-center w-25">
      <label className="me-2" htmlFor="pageLimitSelect">Limit: </label>
      <select
        className="form-control form-control-sm"
        id="pageLimitSelect"
        onChange={(e) => props.changePageLimit(Number(e.target.value))}
        value={props.pageLimit}
      >{(Array.from({ length: 10 }, (_, i) => (i + 1) * 10)).map((limit: number) => (
        <option key={limit} value={limit}>{limit}</option>
      ))}</select>
    </div>
  );
});

export default TableLimiter;