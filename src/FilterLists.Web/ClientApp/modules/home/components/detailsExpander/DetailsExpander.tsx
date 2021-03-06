import * as React from "react";
import { IFilterListDetailsDto } from "./IFilterListDetailsDto";
import { InfoCard } from "./infoCard";
import { LinkButtonGroup } from "./LinkButtonGroup";
import { MaintainersInfoCard } from "./maintainersInfoCard";

export const DetailsExpander = (props: IFilterListDetailsDto) => {
    return <div className="card border-primary">
               <div className="card-body p-2">
                   <div className="container m-0">
                       <div className="row">
                           <InfoCard {...props}/>
                           <LinkButtonGroup {...props}/>
                       </div>
                       <div className="row">
                           <MaintainersInfoCard maintainers={props.maintainers}/>
                       </div>
                   </div>
               </div>
           </div>;
};