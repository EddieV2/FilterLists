import * as React from "react";
import { IListMaintainerDto } from "../IFilterListDetailsDto";
import { MaintainerInfoCard } from "./MaintainerInfoCard";

interface IProps {
    maintainers: IListMaintainerDto[];
}

export const MaintainersInfoCard = (props: IProps) => {
    return props.maintainers.length > 0
               ? <div className="w-100">
                     {props.maintainers.map((m: IListMaintainerDto, i) => <MaintainerInfoCard {...m} key={i}/>)}
                 </div>
               : null;
};