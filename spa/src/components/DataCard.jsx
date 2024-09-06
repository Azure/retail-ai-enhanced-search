import React from 'react';
import ShowMoreText from "react-show-more-text";
import { Card, CardContent, Typography, CardActions, CardHeader, CardMedia, Avatar, IconButton, Collapse, ListItem, List } from '@mui/material';
import { Favorite, Share } from '@mui/icons-material'

export default function DataCard({ item, index }) {

    let storageAccountUrl = `${import.meta.env.VITE_STORAGE_ACCOUNT_URL}`

    const executeOnClick = (isExpanded) => {
    }

    return (
        <Card class="m-0 max-w-72 bg-white rounded-md shadow-xl">
            <CardMedia
                component="img"
                image={`${storageAccountUrl}${item.imageName}`}
                alt="Product Image"
                sx={{ 'border-radius': '6px 6px 0px 0px', padding: "0 0 0 0", margin: "0 0 0 0", minHeight: 0, objectFit: "fill" }}
            />
            <CardContent class="p-2">
                <Typography class="text-lg font-semibold">
                    {item.name}
                </Typography>
                <Typography class="text-sm text-cyan-700 font-semibold">
                    {item.brand}
                </Typography>
                <br />
                <Typography color="text.secondary" class="font-light">
                    <ShowMoreText
                        lines={4}
                        more="Read more"
                        less="Read less"
                        onClick={executeOnClick}
                        expanded={false}
                        truncatedEndingComponent={"... "}
                    >
                        {item.description}
                    </ShowMoreText>
                </Typography>
            </CardContent>
            <br />
            <CardActions disableSpacing class="relative flex p-0 justify-end">
                <Typography variant="body2" color="text.secondary" class="pl-2 text-pretty text-lg font-semibold absolute left-0">
                    ${item.price}
                </Typography>
                <IconButton aria-label="add to favorites" color='error' class="pb-2 fill-current text-red-400 hover:text-red-600">
                    <Favorite />
                </IconButton>
                <IconButton aria-label="share" color='info' class="pb-2 pr-2 fill-current text-blue-400 hover:text-blue-600">
                    <Share />
                </IconButton>

            </CardActions>
        </Card>
    )
}