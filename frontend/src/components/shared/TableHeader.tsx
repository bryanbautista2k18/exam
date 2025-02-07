import React, { memo } from "react";

interface TableHeaderType {
    tableHeaders: { [key: string]: string }[];
}

const TableHeader = memo((props: TableHeaderType): React.JSX.Element => {
  return (
    <thead>
        <tr>
            {props.tableHeaders.map((tableHeader: { [key: string]: string }, i: number) => (
                <th className={tableHeader.className} key={i} scope="col">{tableHeader.text}</th>
            ))}
            <th scope="col"></th>
        </tr>
    </thead>
  );
});

export default TableHeader;