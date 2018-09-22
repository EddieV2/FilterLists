import * as React from "react";
import { ISoftwareDto, ILanguageDto, IListIndexDto } from "../../interfaces";
import ReactTable from "react-table"
import "react-table/react-table.css"
import "./listsTable.css";
import { SoftwareIcon } from "../softwareIcon";
import * as moment from "moment";
import { DetailsExpander } from "../../components";

export interface IColumnVisibility {
    column: string;
    visible: boolean;
}

interface IProps {
    languages: ILanguageDto[];
    lists: IListIndexDto[];
    software: ISoftwareDto[];
    columnVisibility: IColumnVisibility[];
    pageSize: number;
}

export const ListsTable = (props: IProps) => {
    return <ReactTable
               data={props.lists}
               key={props.pageSize}
               defaultPageSize={props.pageSize}
               showPageSizeOptions={false}
               columns={[
                   Name,
                   Software(props),
                   //{
                   //    Header: "Tags",
                   //    accessor: "tags",
                   //    filterable: true,
                   //    filterMethod: (f: any, r: any) =>
                   //        r[f.id].map((e: any) => e.name).join().toUpperCase().includes(f.value.toUpperCase()),
                   //    sortable: false,
                   //    Cell: (c: any) => c.value
                   //                      ? <div className="fl-tag-container">
                   //                            {c.value.map((e: any, i: number) =>
                   //                                <span className="badge"
                   //                                      style={{
                   //                                          backgroundColor: `#${e.colorHex}`,
                   //                                          color: getContrast(`${e.colorHex}`)
                   //                                      }}
                   //                                      title={e.description}
                   //                                      key={i}>
                   //                                    {e.name}
                   //                                </span>)}
                   //                        </div>
                   //                      : null,
                   //    width: 215,
                   //    headerClassName: "d-none d-md-block",
                   //    className: "d-none d-md-block",
                   //    show: props.columnVisibility.filter(
                   //        (c: IColumnVisibility) => { return c.column === "Tags"; })[0].visible
                   //},
                   {
                       Header: "Languages",
                       accessor: "languageIds",
                       filterable: true,
                       filterMethod: (f: any, r: any) =>
                           f.value === "any" ||
                           (r[f.id]
                                ? r[f.id].join(",").split(",").includes(f.value)
                                : false),
                       Filter: ({ filter, onChange }) =>
                           <select
                               onChange={(event: any) => onChange(event.target.value)}
                               style={{ width: "100%" }}
                               value={filter ? filter.value : "any"}>
                               <option value="any">Any</option>
                               {props.languages
                                    ? props.languages.map((l: ILanguageDto, i: number) =>
                                        <option value={l.id} key={i}>{l.name}</option>)
                                    : null}
                           </select>,
                       sortable: false,
                       Cell: (c: any) => c.value
                                         ? <div className="fl-tag-container">
                                               {c.value.map((e: number, i: number) =>
                                                   <span className="badge badge-secondary"
                                                         title={props.languages.filter(l => l.id === e)[0].name}
                                                         key={i}>
                                                       {props.languages.filter(l => l.id === e)[0].iso6391}
                                                   </span>)}
                                           </div>
                                         : null,
                       style: { whiteSpace: "inherit" },
                       width: 95,
                       headerClassName: "d-none d-md-block",
                       className: "d-none d-md-block",
                       show: props.columnVisibility.filter(
                           (c: IColumnVisibility) => { return c.column === "Languages"; })[0].visible
                   },
                   {
                       Header: "Updated",
                       accessor: "updatedDate",
                       sortMethod: (a: any, b: any) =>
                           moment(a).isValid()
                           ? (moment(b).isValid()
                                  ? (moment(a).isBefore(b)
                                         ? -1
                                         : 1)
                                  : 1)
                           : -1,
                       Cell: (c: any) =>
                           <div>
                               {moment(c.value).isValid()
                                    ? moment(c.value).format("l")
                                    : "N/A"}
                           </div>,
                       style: { whiteSpace: "inherit" },
                       width: 100,
                       headerClassName: "d-none d-md-block",
                       className: "d-none d-md-block",
                       show: props.columnVisibility.filter(
                           (c: IColumnVisibility) => { return c.column === "Updated Date"; })[0].visible
                   },
                   {
                       Header: "Details",
                       accessor: "id",
                       sortable: false,
                       expander: true,
                       Expander: ({ isExpanded, row }) =>
                           <div>
                               {isExpanded
                                    ? <button className="btn btn-primary btn-block active"
                                              title={`Collapse details about ${row.name}.`}>
                                          Details
                                      </button>
                                    : <button className="btn btn-primary btn-block"
                                              title={`Learn more about ${row.name}.`}>
                                          Details
                                      </button>}
                           </div>,
                       style: { textAlign: "center" },
                       width: 90
                   }
               ]}
               SubComponent={(r: any) => <DetailsExpander listId={r.original.id}/>}
               className="-striped -highlight"/>;
};

const Name = {
    Header: "Name",
    accessor: "name",
    filterable: true,
    filterMethod: (f: any, r: any) => r[f.id].toUpperCase().includes(f.value.toUpperCase()),
    sortMethod: (a: any, b: any) => a.toUpperCase() > b.toUpperCase() ? 1 : -1,
    Cell: (c: any) => <h2 className="mb-0">{c.value}</h2>
};

const Software = (props: any) => {
    return {
        Header: "Software",
        accessor: "syntaxId",
        filterable: true,
        filterMethod: (f: any, r: any) =>
            f.value === "any" ||
            (r[f.id]
                 ? props.software.filter(s => s.id.includes(r[f.id])).includes(f.value)
                 : false),
        Filter: ({ filter, onChange }: any) =>
            <select
                onChange={(event: any) => onChange(event.target.value)}
                style={{ width: "100%" }}
                value={filter ? filter.value : "any"}>
                <option value="any">Any</option>
                {props.software
                     ? props.software.map(
                         (s: ISoftwareDto, i: number) => <option value={s.id} key={i}>{s.name}</option>)
                     : null}
            </select>,
        sortable: false,
        Cell: (c: any) => c.value
                          ? props.software.filter((s: any) => s.syntaxIds.indexOf(c.value) !== 0).map(
                              (s: ISoftwareDto, i: number) =>
                              <SoftwareIcon id={s.id} key={i}/>)
                          : null,
        width: 155,
        headerClassName: "d-none d-md-block",
        className: "d-none d-md-block",
        show: props.columnVisibility.filter((c: IColumnVisibility) => { return c.column === "Software"; })[0].visible
    };
};