import React, { memo } from "react";

interface TablePaginatorType {
  loading: boolean;
  currentPage: number;
  hasMorePages: boolean;
  getPrevPage: () => void;
  getNextPage: () => void;
}

const TablePaginator = memo((props: TablePaginatorType): React.JSX.Element => {
  return (
    <nav className="d-flex justify-content-end">
      <ul className="pagination pagination-sm m-0">
        <li className="page-item">
          <button
            className={`page-link ${(props.loading || props.currentPage === 1) ? `disabled` : ``}`}
            disabled={(props.loading || props.currentPage === 1)}
            onClick={props.getPrevPage}
            type="button"
          >Prev</button>
        </li>
        <li className="page-item">
          <button
            className={`page-link ${(props.loading || !props.hasMorePages) ? `disabled` : ``}`}
            disabled={(props.loading || !props.hasMorePages)}
            onClick={props.getNextPage}
            type="button"
          >Next</button>
        </li>
      </ul>
    </nav>
  );
});

export default TablePaginator;